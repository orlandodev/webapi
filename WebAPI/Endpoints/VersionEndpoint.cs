using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Endpoints;

public static class VersionEndpoint
{
    /// <summary>
    /// Illustrates an API endpoint allowing anonymous access from all callers.
    /// </summary>
    /// <param name="app"></param>
    public static void MapVersionEndpoint(this WebApplication app)
    {
        app.MapGet("/api/version", [AllowAnonymous] () =>
        {
            return TypedResults.Ok(new { version = "1.0" });
        })
        .WithName("GetApiVersion")
        .WithSummary("Get the API version")
        .WithDescription("Returns the current version of the API.")
        .WithTags("Information")
        .WithOpenApi();
    }
}
