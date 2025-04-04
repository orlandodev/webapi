
namespace WebAPI.Services.DTOs;

public class AlbumReadDTO
{
    public int AlbumId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string ArtistName { get; set; } = string.Empty;
}
