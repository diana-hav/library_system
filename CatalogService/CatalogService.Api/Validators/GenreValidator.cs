using CatalogService.Bll.Dtos;
using FluentValidation;

namespace CatalogService.Api.Validators;

public class GenreValidator : AbstractValidator<GenreDto>
{
    public GenreValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Назва жанру є обов'язковою")
            .MaximumLength(50);
    }
}
