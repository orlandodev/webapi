using Microsoft.EntityFrameworkCore;
using WebAPI.DataAccess.Models;

namespace WebAPI.DataAccess.Repositories;

public class AlbumRepository : IAlbumRepository
{
    private readonly ChinookContext context;

    public AlbumRepository(ChinookContext context)
    {
        this.context = context;
    }

    public async Task<int> AddTracksToAlbumAsync(int albumId, IList<Track> tracks)
    {
        var album = await GetAlbumByIdAsync(albumId);

        if (album == null) 
        { 
            return 0;
        }

        album.Tracks = tracks;
        context.Albums.Update(album);
        
        var trackedAdded = await context.SaveChangesAsync();

        return trackedAdded;
    }

    public async Task<Album> CreateAlbumAsync(Album album)
    {
        context.Albums.Add(album);
        await context.SaveChangesAsync();

        return album;
    }

    public async Task<int> DeleteAlbumByIdAsync(int albumid)
    {
        var affected = await context.Albums
            .Where(model => model.AlbumId == albumid)
            .ExecuteDeleteAsync();

        return affected;
    }

    public async Task<Album?> GetAlbumByIdAsync(int id)
    {
        return await context.Albums.Include(a => a.Artist).AsNoTracking()
            .FirstOrDefaultAsync(model => model.AlbumId == id);
    }

    public async Task<IList<Album>> GetAlbumsAsync()
    {
        return await context.Albums.Include(a => a.Artist).AsNoTracking().ToArrayAsync();
    }

    public Task<int> GetArtistIdByNameAsync(string name)
    {
        var artist = context.Artists.AsNoTracking().Where(x => x.Name == name).FirstOrDefault();

        if (artist == null)
        {
            return Task.FromResult(0);
        }

        return Task.FromResult(artist.ArtistId);
    }

    public async Task<Album?> UpdateAlbumAsync(Album album)
    {
        var affected = await context.Albums
            .Where(model => model.AlbumId == album.AlbumId)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(m => m.Title, album.Title)
                .SetProperty(m => m.ArtistId, album.ArtistId)
                );
        context.SaveChanges();

        return await GetAlbumByIdAsync(album.AlbumId);
    }
}
