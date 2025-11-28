using CatalogService.Bll.Dtos;
using FluentValidation;

namespace CatalogService.Api.Validators;

public class UpdateBookValidator : AbstractValidator<UpdateBookDto>
{
    public UpdateBookValidator()
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



