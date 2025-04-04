using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebAPI.Services.DTOs;

public class AlbumUpdateDTO
{
    //[Required]
    public int AlbumId { get; set; }

    //[MaxLength(160)]
    public string? Title { get; set; }

    //[MaxLength(120)]
    public string? ArtistName { get; set; }

    [JsonIgnore]
    public int ArtistId { get; set; }
}
