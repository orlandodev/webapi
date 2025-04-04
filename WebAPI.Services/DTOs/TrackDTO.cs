
namespace WebAPI.Services.DTOs
{
    public class TrackDTO
    {
        public required string Name { get; set; }

        public int AlbumId { get; set; }

        public int MediaTypeId { get; set; } = 1;   // MPEG audio file

        public string Composer { get; set; } = string.Empty;

        public int Milliseconds { get; set; }

        public double UnitPrice { get; set; } = .99;
    }
}
