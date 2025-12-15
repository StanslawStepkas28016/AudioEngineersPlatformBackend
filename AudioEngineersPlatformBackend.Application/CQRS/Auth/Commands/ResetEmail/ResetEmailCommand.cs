using MediatR;

namespace AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.ResetEmail;

public class ResetEmailCommand : IRequest<ResetEmailCommandResult>
{
    public required Guid IdUser { get; set; }
    public required string NewEmail { get; set; } 
}