using MediatR;

namespace AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.Logout;

public class LogoutCommand : IRequest<LogoutCommandResult>
{
    public required Guid IdUser { get; set; }
    public required string RefreshToken { get; set; } 
}