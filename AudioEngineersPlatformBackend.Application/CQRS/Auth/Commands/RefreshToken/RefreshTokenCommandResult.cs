using AudioEngineersPlatformBackend.Domain.Entities;

namespace AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.RefreshToken;

public class RefreshTokenCommandResult
{
    public required User User { get; set; }
}