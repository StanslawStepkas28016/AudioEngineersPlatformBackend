using FluentValidation;

namespace AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.VerifyAccount;

public class VerifyAccountCommandValidator : AbstractValidator<VerifyAccountCommand>
{
    public VerifyAccountCommandValidator()
    {
        RuleFor(exp => exp.VerificationCode)
            .NotEmpty()
            .WithMessage("Verification code must be provided.");
    }
}