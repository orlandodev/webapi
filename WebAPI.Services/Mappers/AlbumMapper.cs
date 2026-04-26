using Riok.Mapperly.Abstractions;
using WebAPI.DataAccess.Models;
using WebAPI.Services.DTOs;

namespace WebAPI.Services.Mappers;

[Mapper]
public static partial class AlbumMapper
{
    [MapperIgnoreSource(nameof(AlbumCreateDTO.ArtistName))]
    [MapperIgnoreTarget(nameof(Album.AlbumId))]
    [MapperIgnoreTarget(nameof(Album.Artist))]
    [MapperIgnoreTarget(nameof(Album.Tracks))]
    public static partial Album Map(AlbumCreateDTO dto);

    [MapperIgnoreSource(nameof(AlbumUpdateDTO.ArtistName))]
    [MapperIgnoreTarget(nameof(Album.Artist))]
    [MapperIgnoreTarget(nameof(Album.Tracks))]
    public static partial Album Map(AlbumUpdateDTO dto);

    [MapperIgnoreSource(nameof(Album.ArtistId))]
    [MapperIgnoreSource(nameof(Album.Tracks))]
    [MapProperty("Artist.Name", nameof(AlbumReadDTO.ArtistName))]
    public static partial AlbumReadDTO Map(Album album);

    public static partial IList<AlbumReadDTO> Map(IList<Album> albums);
}
