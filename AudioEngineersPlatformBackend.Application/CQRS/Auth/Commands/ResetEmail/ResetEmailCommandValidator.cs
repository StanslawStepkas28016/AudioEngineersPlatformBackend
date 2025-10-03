using FluentValidation;

namespace AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.ResetEmail;

public class ResetEmailCommandValidator : AbstractValidator<ResetEmailCommand>
{
    public ResetEmailCommandValidator()
    {
        RuleFor(exp => exp.IdUser)
            .NotEmpty()
            .WithMessage("IdUser must be provided.");

        RuleFor(exp => exp.NewEmail)
            .EmailAddress()
            .WithMessage("Invalid email.");
    }
}