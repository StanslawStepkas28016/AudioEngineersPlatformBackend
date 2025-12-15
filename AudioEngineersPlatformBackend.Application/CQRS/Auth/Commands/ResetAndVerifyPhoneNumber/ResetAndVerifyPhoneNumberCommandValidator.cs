using FluentValidation;

namespace AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.ResetAndVerifyPhoneNumber;

public class ResetAndVerifyPhoneNumberCommandValidator : AbstractValidator<ResetAndVerifyPhoneNumberCommand>
{
    public ResetAndVerifyPhoneNumberCommandValidator()
    {
        RuleFor(exp => exp.IdUser)
            .NotEmpty()
            .WithMessage("IdUser must be provided.");

        RuleFor(exp => exp.NewPhoneNumber)
            .NotEmpty()
            .WithMessage("New phone number must be provided.");
    }
}