using WebAPI.DataAccess.Models;

namespace WebAPI.DataAccess;

public interface IAlbumRepository
{
    public Task<IList<Album>> GetAlbumsAsync();

    public Task<Album?> GetAlbumByIdAsync(int id);

    public Task<Album> CreateAlbumAsync(Album album);

    public Task<Album?> UpdateAlbumAsync(Album album);

    public Task<int> DeleteAlbumByIdAsync(int albumId);

    public Task<int> AddTracksToAlbumAsync(int albumId, IList<Track> tracks);

    public Task<int> GetArtistIdByNameAsync(string name);
}
