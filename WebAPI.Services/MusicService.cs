using AutoMapper;
using WebAPI.DataAccess;
using WebAPI.DataAccess.Models;
using WebAPI.Services.DTOs;

namespace WebAPI.Services;

public class MusicService(IAlbumRepository albumRepository, IMapper mapper) : IMusicService
{
    private readonly IAlbumRepository albumRepository = albumRepository;
    private readonly IMapper mapper = mapper;

    public async Task<AlbumReadDTO?> CreateAlbumAsync(AlbumCreateDTO albumCreateDTO)
    {
        var artistId = await albumRepository.GetArtistIdByNameAsync(albumCreateDTO.ArtistName);

        if (artistId == 0) 
        {
            return null;
        }

        albumCreateDTO.ArtistId = artistId;

        var album = mapper.Map<Album>(albumCreateDTO);
        var addedAlbum = await albumRepository.CreateAlbumAsync(album);

        if (addedAlbum != null) 
        {
            return new AlbumReadDTO
            {
                Title = addedAlbum.Title,
                ArtistName = albumCreateDTO.ArtistName,
                AlbumId = addedAlbum.AlbumId
            };
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

        return album != null ? new AlbumReadDTO { AlbumId = album.AlbumId, ArtistName = album.Artist.Name, Title = album.Title } : null;
    }

    public async Task<IList<AlbumReadDTO>> GetAlbumsAsync()
    {
        var albums = await albumRepository.GetAlbumsAsync();

        var albumsCollection = new List<AlbumReadDTO>();

        foreach (var album in albums) 
        { 
            albumsCollection.Add(new AlbumReadDTO { AlbumId = album.AlbumId, Title = album.Title, ArtistName = album.Artist.Name });
        }

        return albumsCollection;
    }

    public async Task<AlbumReadDTO> UpdateAlbumAsync(AlbumUpdateDTO albumUpdateDTO)
    {
        int artistId;

        if (albumUpdateDTO.ArtistName != null)
        {
            artistId = await albumRepository.GetArtistIdByNameAsync(albumUpdateDTO.ArtistName);
            albumUpdateDTO.ArtistId = artistId;
        }

        var album = mapper.Map<Album>(albumUpdateDTO);

        var updatedAlbum = await albumRepository.UpdateAlbumAsync(album);

        if (updatedAlbum != null)
        {
            return new AlbumReadDTO
            {
                Title = updatedAlbum.Title,
                ArtistName = updatedAlbum.Artist.Name,
                AlbumId = updatedAlbum.AlbumId
            };
        }

        return null;
    }
}
