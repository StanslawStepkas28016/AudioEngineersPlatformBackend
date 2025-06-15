namespace AudioEngineersPlatformBackend.Contracts.Auth;

public record RegisterResponse(
    Guid IdUser,
    string Email,
    string Token
);