using FluentValidation;
using WebAPI.Services.DTOs;

namespace WebAPI.Validation
{
    /// <summary>
    /// An example of using fluent validation vs. the traditional data annotations approach.
    /// </summary>
    public class CreateAlbumValidator : AbstractValidator<AlbumCreateDTO>
    {
        public CreateAlbumValidator()
        {
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.ArtistName).NotEmpty();
            RuleFor(x => x.Title).MaximumLength(160);
            RuleFor(x => x.ArtistName).MaximumLength(120);
        }
    }
}
