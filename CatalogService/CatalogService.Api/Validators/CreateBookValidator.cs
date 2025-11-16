using CatalogService.Bll.Dtos;
using FluentValidation;

namespace CatalogService.Api.Validators;

public class CreateBookValidator : AbstractValidator<CreateBookDto>
{
    public CreateBookValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Назва книги є обов'язковою")
            .MaximumLength(200).WithMessage("Назва книги не може бути довшою за 200 символів");

        RuleFor(x => x.AuthorId)
            .GreaterThan(0).WithMessage("Потрібно вказати дійсного автора");

        RuleFor(x => x.GenreId)
            .GreaterThan(0).WithMessage("Потрібно вказати дійсний жанр");
    }
}
