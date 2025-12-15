using MediatR;

namespace AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.ForgotPassword;

public class ForgotPasswordCommand : IRequest<ForgotPasswordCommandResult>
{
    public required string Email { get; set; } 
}