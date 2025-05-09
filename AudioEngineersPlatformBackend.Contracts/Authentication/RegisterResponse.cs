namespace AudioEngineersPlatformBackend.Contracts.Authentication;

public record RegisterResponse(
    Guid IdUser,
    string Email,
    string Token
);