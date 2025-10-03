using FluentValidation;

namespace AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.Logout;

public class LogoutCommandValidator : AbstractValidator<LogoutCommand>
{
    public LogoutCommandValidator()
    {
        RuleFor(exp => exp.IdUser)
            .NotEmpty()
            .WithMessage("IdUser must be provided.");

        RuleFor(exp => exp.RefreshToken)
            .NotEmpty()
            .WithMessage("RefreshToken must be provided.");
    }
}