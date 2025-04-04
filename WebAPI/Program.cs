using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebAPI.DataAccess;
using WebAPI.DataAccess.Repositories;
using WebAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using System.Security.Principal;
using Scalar.AspNetCore;
using WebAPI.Endpoints;
using Microsoft.AspNetCore.Authorization;
using System.Threading.RateLimiting;
using Microsoft.IdentityModel.Logging;
using WebAPI.Authorization;
using FluentValidation;
using WebAPI.Validation;
using WebAPI.Services.DTOs;
using Asp.Versioning;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();

// Add services to the container.
builder.Services.AddOpenApi();

builder.Services.AddDbContext<ChinookContext>(options =>
{   
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddApiVersioning(options =>
{
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new QueryStringApiVersionReader("api-version"),
        new HeaderApiVersionReader("x-api-Version"));
});

#region authentication

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Audience = builder.Configuration["Authentication:Schemes:Bearer:ValidAudiences:0"];
        options.RequireHttpsMetadata = false;
        options.IncludeErrorDetails = true;

        var signingKey = builder.Configuration["Authentication:Schemes:Bearer:SigningKeys:0:Value"];
        // check your secrets.json file to make sure the value is there before running this
        var signingKeyBytes = Convert.FromBase64String(signingKey!);

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Authentication:Schemes:Bearer:ValidIssuer"],
            ValidAudience = builder.Configuration["Authentication:Schemes:Bearer:ValidAudiences:0"],
            IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes)
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                //Log failed authentications
                Console.WriteLine(context.Exception.Message);
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine(context.Request.Headers.Authorization);

                //Log successful authentications
                if (context.Principal is ClaimsPrincipal && context.Principal.Identity is IIdentity)
                {
                    Console.WriteLine("Is authenticated? {0}", context.Principal.Identity.IsAuthenticated);
                }
                return Task.CompletedTask;
            }
        };
    })
    .AddJwtBearer("LocalIdentityServer", options =>
    {
        options.MetadataAddress = "https://localhost:5001/.well-known/openid-configuration"; // discovery e/p from auth server
        // Optional if the MetadataAddress is specified
        options.Audience = builder.Configuration["Authentication:Schemes:LocalIdentityServer:ValidAudiences:0"];   // a scope
        options.MapInboundClaims = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
        };

        options.IncludeErrorDetails = true;

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                //Log failed authentications
                Console.WriteLine(context.Exception.Message);
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine(context.Request.Headers.Authorization);

                //Log successful authentications
                if (context.Principal is ClaimsPrincipal && context.Principal.Identity is IIdentity)
                {
                    Console.WriteLine("Is authenticated? {0}", context.Principal.Identity.IsAuthenticated);
                }
                return Task.CompletedTask;
            }
        };
    });

#endregion

#region authorization

// acceptable when there's only one token issuer
//builder.Services.AddAuthorization();

// enable support for multiple token issuers with no specific policies
//builder.Services.AddAuthorization(options =>
//{
//    var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
//        JwtBearerDefaults.AuthenticationScheme,
//        "LocalIdentityServer");
//    defaultAuthorizationPolicyBuilder =
//        defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
//    options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
//});

builder.Services.AddSingleton<IAuthorizationHandler, ScopeAuthorizationHandler>();

// add specific policies for our API endpoints
builder.Services.AddAuthorizationBuilder()
    .AddPolicy(Constants.AlbumApiAdminPolicy, policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", [Constants.WebApiAdminScope]);
        policy.AddAuthenticationSchemes([JwtBearerDefaults.AuthenticationScheme, "LocalIdentityServer"]);
    })
    .AddPolicy(Constants.AlbumApiReadOrAdminPolicy, policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.AddAuthenticationSchemes([JwtBearerDefaults.AuthenticationScheme, "LocalIdentityServer"]);
        policy.Requirements.Add(new ScopeRequirement());
    });

#endregion

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<IAlbumRepository, AlbumRepository>();
builder.Services.AddScoped<IMusicService, MusicService>();

builder.Services.AddScoped<IValidator<AlbumCreateDTO>, CreateAlbumValidator>();
builder.Services.AddScoped<IValidator<AlbumUpdateDTO>, UpdateAlbumValidator>();

#region rate limiting

// add rate limiting support
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(),
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 10,
                QueueLimit = 0,
                Window = TimeSpan.FromMinutes(1)
            }));

    options.OnRejected = async (context, cancellationToken) =>
    {
        // Custom rejection handling logic
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        context.HttpContext.Response.Headers["Retry-After"] = "60";

        await context.HttpContext.Response.WriteAsync("Rate limit exceeded. Please try again later.", cancellationToken);

        // Optional logging
        Console.WriteLine($"Rate limit exceeded for IP: {context.HttpContext.Connection.RemoteIpAddress}");
    };
});

#endregion

var app = builder.Build();

#region error handling

app.UseExceptionHandler();
app.UseStatusCodePages();

#endregion

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    IdentityModelEventSource.ShowPII = true;
}

app.UseHttpsRedirection();

// Not required, but can be enabled to make it explicit
//app.UseAuthentication();
//app.UseAuthorization();

app.MapAlbumEndpoints();
app.MapVersionEndpoint();

// for illustrative purposes
app.MapSampleExceptionEndpoint();

app.UseRateLimiter();

app.Run();
