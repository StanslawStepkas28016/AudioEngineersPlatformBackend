using MediatR;

namespace AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.ResetPassword;

public class ResetPasswordCommand : IRequest<ResetPasswordCommandResult>
{
    public required Guid IdUser { get; set; }
    public required string CurrentPassword { get; init; } 
    public required string NewPassword { get; init; } 
    public required string NewPasswordRepeated { get; init; } 
}