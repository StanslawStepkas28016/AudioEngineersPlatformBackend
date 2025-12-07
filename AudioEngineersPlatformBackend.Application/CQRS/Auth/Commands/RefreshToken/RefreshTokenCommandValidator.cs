using FluentValidation;
using MediatR;

namespace AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.RefreshToken;

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(exp => exp.RefreshToken)
            .NotEmpty()
            .WithMessage("Refresh token must be provided.");
    }
}