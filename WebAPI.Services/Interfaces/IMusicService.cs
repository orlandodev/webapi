using WebAPI.Services.DTOs;

namespace WebAPI.Services;

public interface IMusicService
{
    public Task<IList<AlbumReadDTO>> GetAlbumsAsync();

    public Task<AlbumReadDTO?> GetAlbumByIdAsync(int albumId);

    public Task<AlbumReadDTO?> CreateAlbumAsync(AlbumCreateDTO albumCreateDTO);

    public Task<AlbumReadDTO> UpdateAlbumAsync(AlbumUpdateDTO albumUpdateDTO);

    public Task<int> DeleteAlbumByIdAsync(int albumId);
}
