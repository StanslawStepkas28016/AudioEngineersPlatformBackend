using FluentValidation;

namespace AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.VerifyForgotPassword;

public class VerifyForgotPasswordCommandValidator : AbstractValidator<VerifyForgotPasswordCommand>
{
    public VerifyForgotPasswordCommandValidator()
    {
        RuleFor(exp => exp.ForgotPasswordToken)
            .NotEmpty()
            .WithMessage($"{nameof(VerifyForgotPasswordCommand.ForgotPasswordToken)} must be provided.");

        RuleFor(exp => exp.NewPassword)
            .NotEmpty()
            .WithMessage($"{nameof(VerifyForgotPasswordCommand.NewPassword)} must be provided.");

        RuleFor(exp => exp.NewPasswordRepeated)
            .NotEmpty()
            .WithMessage($"{nameof(VerifyForgotPasswordCommand.NewPasswordRepeated)} must be provided.");

        RuleFor(exp => exp.NewPassword)
            .Equal(exp => exp.NewPasswordRepeated)
            .WithMessage("Both password must be matching.");
    }
}