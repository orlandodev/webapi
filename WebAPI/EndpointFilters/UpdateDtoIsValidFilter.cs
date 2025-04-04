
using WebAPI.Services.DTOs;

namespace WebAPI.EndpointFilters;

/// <summary>
/// Refer to: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/min-api-filters?view=aspnetcore-9.0
/// and https://github.com/dotnet/AspNetCore.Docs/tree/main/aspnetcore/fundamentals/minimal-apis/min-api-filters/7samples
/// </summary>
public class UpdateDtoIsValidFilter : IEndpointFilter
{
    private ILogger _logger;

    public UpdateDtoIsValidFilter(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<UpdateDtoIsValidFilter>();
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var artistId = context.GetArgument<int>(0);
        var updateDto = context.GetArgument<AlbumUpdateDTO>(1);

        var errors = new List<KeyValuePair<string, string[]>>();

        if (artistId <= 0) 
        {
            errors.Add(new ("artistId", ["Artist Id cannot be less than or equal to zero"]));
        }

        if (string.IsNullOrEmpty(updateDto.ArtistName))
        {
            errors.Add(new("artistName", ["'ArtistName' cannot be empty"]));
        }

        if (string.IsNullOrEmpty(updateDto.Title))
        {
            errors.Add(new ("title", ["'Title' cannot be empty"]));
        }

        if (updateDto.ArtistName?.Length > 120)
        {
            errors.Add(new ("artistName.length", ["'ArtistName' exceeds max length of 120 characters"]));
        }

        if (updateDto.Title?.Length > 160)
        {
            errors.Add(new ("title.length", ["'Title' exceeds max length of 160 characters"]));
        }

        if (errors.Count > 0)
        {
            return Results.ValidationProblem(errors.ToDictionary());
        }

        return await next(context);
    }
}
