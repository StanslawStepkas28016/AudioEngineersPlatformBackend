using FluentValidation;

namespace AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(exp => exp.Email)
            .EmailAddress()
            .WithMessage("Email must be provided.");

        RuleFor(exp => exp.Password)
            .NotEmpty()
            .WithMessage("Password must be provided.");
    }
}