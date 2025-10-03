using FluentValidation;

namespace AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.VerifyResetPassword;

public class VerifyResetPasswordValidator : AbstractValidator<VerifyResetPasswordCommand>
{
    public VerifyResetPasswordValidator()
    {
    }
}