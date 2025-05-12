namespace AudioEngineersPlatformBackend.Contracts.Authentication;

public record LoginResponse(
    Guid IdUser,
    string Token
);