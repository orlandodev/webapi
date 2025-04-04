using FluentValidation;
using WebAPI.Services.DTOs;

namespace WebAPI.Validation
{
    public class UpdateAlbumValidator : AbstractValidator<AlbumUpdateDTO>
    {
        public UpdateAlbumValidator()
        {
            RuleFor(x => x.AlbumId).GreaterThan(0);
            RuleFor(x => x.Title).MaximumLength(160);
            RuleFor(x => x.ArtistName).MaximumLength(120);
            RuleFor(x => x.ArtistName).NotEmpty();
            RuleFor(x => x.Title).NotEmpty();
        }
    }
}
