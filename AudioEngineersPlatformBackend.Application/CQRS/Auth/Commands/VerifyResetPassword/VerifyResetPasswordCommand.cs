using MediatR;

namespace AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.VerifyResetPassword;

public class VerifyResetPasswordCommand : IRequest<VerifyResetPasswordCommandResult>
{
    public required Guid ResetPasswordToken { get; set; }
}