using MediatR;

namespace AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.Login;

public class LoginCommand : IRequest<LoginCommandResult>
{
    public required string Email { get; init; } 
    public required string Password { get; init; } 
}