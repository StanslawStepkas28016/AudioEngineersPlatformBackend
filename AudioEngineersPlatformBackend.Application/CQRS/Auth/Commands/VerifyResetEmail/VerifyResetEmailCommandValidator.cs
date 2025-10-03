using FluentValidation;

namespace AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.VerifyResetEmail;

public class VerifyResetEmailCommandValidator : AbstractValidator<VerifyResetEmailCommand>
{
    public VerifyResetEmailCommandValidator()
    {
        RuleFor(exp => exp.ResetEmailToken)
            .NotEmpty()
            .WithMessage("ResetEmailToken must be provided.");
    }
}