using FluentValidation;

namespace AudioEngineersPlatformBackend.Application.CQRS.Advert.Commands.ChangeAdvertData;

public class ChangeAdvertDataCommandValidator : AbstractValidator<ChangeAdvertDataCommand>
{
    public ChangeAdvertDataCommandValidator()
    {
        RuleFor(exp => exp.IdUser)
            .NotEmpty()
            .WithMessage("IdUser must be provided.");

        RuleFor(exp => exp.IdAdvert)
            .NotEmpty()
            .WithMessage("IdAdvert must be provided.");

        // All remaining data is not validated as its optional.
    }
}