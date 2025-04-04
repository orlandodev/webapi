using System.Text.Json.Serialization;

namespace WebAPI.Services.DTOs;

public class AlbumCreateDTO
{
    public required string Title { get; set; }

    public required string ArtistName { get; set; }

    [JsonIgnore]
    public int ArtistId { get; set; }
}
