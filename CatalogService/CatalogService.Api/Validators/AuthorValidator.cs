using CatalogService.Bll.Dtos;
using FluentValidation;

namespace CatalogService.Api.Validators;

public class AuthorValidator : AbstractValidator<AuthorDto>
{
    public AuthorValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Ім'я автора є обов'язковим")
            .MaximumLength(100);
    }
}
