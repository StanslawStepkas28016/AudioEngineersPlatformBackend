using FluentValidation;

namespace AudioEngineersPlatformBackend.Application.CQRS.Advert.Commands.DeleteAdvert;

public class DeleteAdvertCommandValidator : AbstractValidator<DeleteAdvertCommand>
{
    public DeleteAdvertCommandValidator()
    {
        RuleFor(exp => exp.IdUser)
            .NotEmpty()
            .WithMessage("IdUser must be provided.");

        RuleFor(exp => exp.IdAdvert)
            .NotEmpty()
            .WithMessage("IdAdvert must be provided.");
    }
}