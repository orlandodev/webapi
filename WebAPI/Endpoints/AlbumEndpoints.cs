using Microsoft.AspNetCore.Http.HttpResults;
using WebAPI.Services;
using WebAPI.Services.DTOs;
using FluentValidation;
using Asp.Versioning.Builder;
using Asp.Versioning;

namespace WebAPI.Endpoints;

/// <summary>
/// Exposes endpoints for basic CRUD operations for an Album and related entities.
/// </summary>
public static class AlbumEndpoints
{
    /// <summary>
    ///  The collection of endpoints to get and maintain albums.
    /// </summary>
    /// <param name="routes">The <see cref="IEndpointRouteBuilder"/> instance to add routes to.</param>
    public static void MapAlbumEndpoints(this IEndpointRouteBuilder routes)
    {
        ApiVersionSet apiVersionSet = routes.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1.0))
            .ReportApiVersions()
            .Build();

        var group = routes.MapGroup("/api/Album")
            .WithTags("Album")
            .RequireAuthorization()
            .WithApiVersionSet(apiVersionSet);


        group.MapGet("/", async (IMusicService musicService) =>
        {
            var albums = await musicService.GetAlbumsAsync();

            return albums;
        })
        .WithName("GetAllAlbums")
        .WithSummary("Get a list of albums")
        .WithDescription("Returns a list of all album titles and associated artist name.")
        .WithOpenApi()
        .RequireAuthorization([Authorization.Constants.AlbumApiReadOrAdminPolicy]);


        group.MapGet("/{albumid}", async Task<Results<Ok<AlbumReadDTO>, NotFound>> (int albumid, IMusicService musicService) =>
        {
            return await musicService.GetAlbumByIdAsync(albumid)
                is AlbumReadDTO model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetAlbumById")
        .WithSummary("Get a specific album")
        .WithDescription("Return details for a specific album including title and artist name")
        .WithOpenApi()
        .RequireAuthorization([Authorization.Constants.AlbumApiReadOrAdminPolicy]);

        // update
        group.MapPut("/{albumid}", async (
            int albumid, 
            AlbumUpdateDTO album, 
            IMusicService musicService, 
            IValidator<AlbumUpdateDTO> validator) =>
        {
            album.AlbumId = albumid;
            var validationResult = validator.Validate(album);

            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var updatedAlbum = await musicService.UpdateAlbumAsync(album);

            return updatedAlbum != null ? TypedResults.Ok(updatedAlbum) : TypedResults.NotFound();
        })
        .WithName("UpdateAlbum")
        .WithSummary("Updates a specific album")
        .WithDescription("Updates the details for a specific album including title and artist name")
        .WithOpenApi()
        .RequireAuthorization(Authorization.Constants.AlbumApiAdminPolicy);

        // optional: validate the incoming DTO with a custom filter vs. fluent validation above
        //.AddEndpointFilter<UpdateDtoIsValidFilter>();

        // create
        group.MapPost("/", async (AlbumCreateDTO album, IMusicService musicService, IValidator<AlbumCreateDTO> validator) =>
        {
            // validate the incoming DTO using a fluent validator
            var validationResult = validator.Validate(album);

            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var newAlbum = await musicService.CreateAlbumAsync(album);

            return TypedResults.Created($"/api/Album/{newAlbum?.AlbumId}", newAlbum);
        })
        .WithName("CreateAlbum")
        .WithSummary("Creates a new album")
        .WithDescription("Creates a new album with the supplied title and artist name")
        .WithOpenApi()
        .RequireAuthorization(Authorization.Constants.AlbumApiAdminPolicy);

        // delete
        group.MapDelete("/{albumid}", async Task<Results<NoContent, NotFound>> (int albumid, IMusicService musicService) =>
        {
            var affected = await musicService.DeleteAlbumByIdAsync(albumid);

            return affected == 1 ? TypedResults.NoContent() : TypedResults.NotFound();
        })
        .WithName("DeleteAlbum")
        .WithSummary("Deletes a specific album")
        .WithDescription("Removes an album using its unique Id value")
        .WithOpenApi()
        .RequireAuthorization(Authorization.Constants.AlbumApiAdminPolicy);
    }
}
