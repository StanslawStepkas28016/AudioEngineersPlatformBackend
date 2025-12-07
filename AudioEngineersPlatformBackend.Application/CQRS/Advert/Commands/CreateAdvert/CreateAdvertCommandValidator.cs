using FluentValidation;

namespace AudioEngineersPlatformBackend.Application.CQRS.Advert.Commands.CreateAdvert;

public class CreateAdvertCommandValidator : AbstractValidator<CreateAdvertCommand>
{
    public CreateAdvertCommandValidator()
    {
        RuleFor(exp => exp.IdUser)
            .NotEmpty()
            .WithMessage("IdUser must be provided.");

        RuleFor(exp => exp.Title)
            .NotEmpty()
            .WithMessage("Title must be provided.");

        RuleFor(exp => exp.Description)
            .NotEmpty()
            .WithMessage("Description must be provided.");

        RuleFor(exp => exp.CoverImageFile)
            .NotEmpty()
            .WithMessage("Cover image file must be provided.");

        RuleFor(exp => exp.PortfolioUrl)
            .NotEmpty()
            .WithMessage("Portfolio URL must be provided.");

        RuleFor(exp => exp.Price)
            .NotEmpty()
            .WithMessage("Price must be provided.");

        RuleFor(exp => exp.CategoryName)
            .NotEmpty()
            .WithMessage("Category name must be provided.");
    }
}