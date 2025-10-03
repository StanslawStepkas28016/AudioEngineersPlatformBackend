using FluentValidation;

namespace AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.ResetPassword;

public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(exp => exp.IdUser)
            .NotEmpty()
            .WithMessage("IdUser must be provided.");

        RuleFor(exp => exp.CurrentPassword)
            .NotEmpty()
            .WithMessage("Current password must be provided.");

        RuleFor(exp => exp.NewPassword)
            .NotEmpty()
            .WithMessage("New password must be provided.");

        RuleFor(exp => exp.NewPasswordRepeated)
            .NotEmpty()
            .WithMessage("New password repeated must be provided.");
    }
}