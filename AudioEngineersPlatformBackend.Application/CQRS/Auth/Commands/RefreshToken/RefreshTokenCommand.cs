using MediatR;

namespace AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.RefreshToken;

public class RefreshTokenCommand : IRequest<RefreshTokenCommandResult>
{
    public required Guid RefreshToken { get; set; }
}