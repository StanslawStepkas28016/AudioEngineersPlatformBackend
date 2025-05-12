namespace AudioEngineersPlatformBackend.Contracts.Authentication;

public record LoginRequest(
    string Email,
    string Password
);