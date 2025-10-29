using FluentValidation;

namespace AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.ForgotPassword;

public class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
{
    public ForgotPasswordCommandValidator()
    {
        RuleFor(exp => exp.Email)
            .EmailAddress()
            .WithMessage("Invalid email.");
    }
}