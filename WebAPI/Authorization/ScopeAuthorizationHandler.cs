using IdentityModel;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Authorization;

/// <summary>
/// Allows the use of an "or" condition for evaluating scoped access to GET endpoints.
/// </summary>
public class ScopeAuthorizationHandler : AuthorizationHandler<ScopeRequirement>
{
    private readonly ILogger<ScopeAuthorizationHandler> _logger;

    public ScopeAuthorizationHandler(ILogger<ScopeAuthorizationHandler> logger)
    {
        _logger = logger;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ScopeRequirement requirement)
    {
        var scopeClaims = context.User.Claims.Where(c => c.Type == JwtClaimTypes.Scope);

        if (scopeClaims.Any())
        {
            foreach (var scopeClaim in scopeClaims)
            {
                if (scopeClaim.Value == Constants.WebApiReadScope || scopeClaim.Value == Constants.WebApiAdminScope)
                {
                    context.Succeed(requirement);
                }
            }
        }

        _logger.LogInformation("No scope claims were present.");

        return Task.CompletedTask;
    }
}
