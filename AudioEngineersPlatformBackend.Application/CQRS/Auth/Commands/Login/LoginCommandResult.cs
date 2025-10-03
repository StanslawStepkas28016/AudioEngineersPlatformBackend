using AudioEngineersPlatformBackend.Domain.Entities;

namespace AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.Login;

public class LoginCommandResult
{
    public required User User { get; set; }
}