using MediatR;

namespace AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.VerifyForgotPassword;

public class VerifyForgotPasswordCommand : IRequest<VerifyForgotPasswordCommandResult>
{
    public required Guid ForgotPasswordToken { get; set; }
    public required string NewPassword { get; set; } 
    public required string NewPasswordRepeated { get; set; } 
}