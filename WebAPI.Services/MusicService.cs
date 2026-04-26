using WebAPI.DataAccess;
using WebAPI.Services.DTOs;
using WebAPI.Services.Mappers;

namespace WebAPI.Services;

public class MusicService(IAlbumRepository albumRepository) : IMusicService
{
    private readonly IAlbumRepository albumRepository = albumRepository;

    public async Task<AlbumReadDTO?> CreateAlbumAsync(AlbumCreateDTO albumCreateDTO)
    {
        var artistId = await albumRepository.GetArtistIdByNameAsync(albumCreateDTO.ArtistName);

        if (artistId == 0) 
        {
            return null;
        }

        albumCreateDTO.ArtistId = artistId;

        var album = AlbumMapper.Map(albumCreateDTO);
        var addedAlbum = await albumRepository.CreateAlbumAsync(album);

        if (addedAlbum != null) 
        {
            return AlbumMapper.Map(addedAlbum);
        }

        return null;
    }

    public async Task<int> DeleteAlbumByIdAsync(int albumId)
    {
        return await albumRepository.DeleteAlbumByIdAsync(albumId);
    }

    public async Task<AlbumReadDTO?> GetAlbumByIdAsync(int albumId)
    {
        var album = await albumRepository.GetAlbumByIdAsync(albumId);

        return album != null ? AlbumMapper.Map(album) : null;
    }

    public async Task<IList<AlbumReadDTO>> GetAlbumsAsync()
    {
        var albums = await albumRepository.GetAlbumsAsync();

        return AlbumMapper.Map(albums);
    }

    public async Task<AlbumReadDTO?> UpdateAlbumAsync(AlbumUpdateDTO albumUpdateDTO)
    {
        int artistId;

        if (albumUpdateDTO.ArtistName != null)
        {
            artistId = await albumRepository.GetArtistIdByNameAsync(albumUpdateDTO.ArtistName);
            albumUpdateDTO.ArtistId = artistId;
        }

        var album = AlbumMapper.Map(albumUpdateDTO);

        var updatedAlbum = await albumRepository.UpdateAlbumAsync(album);

        if (updatedAlbum != null)
        {
            return AlbumMapper.Map(updatedAlbum);
        }

        return null;
    }
}
