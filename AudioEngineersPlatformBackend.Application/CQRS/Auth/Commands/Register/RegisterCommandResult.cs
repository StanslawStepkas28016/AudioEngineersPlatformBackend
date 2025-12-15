namespace AudioEngineersPlatformBackend.Application.CQRS.Auth.Commands.Register;

public class RegisterCommandResult
{
    public required Guid IdUser { get; init; }
    public required string Email { get; init; } 
}