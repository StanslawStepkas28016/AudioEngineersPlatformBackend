using MediatR;

namespace AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.VerifyResetEmail;

public class VerifyResetEmailCommand : IRequest<VerifyResetEmailCommandResult>
{
    public required Guid ResetEmailToken { get; set; }
}