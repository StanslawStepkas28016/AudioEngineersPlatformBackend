namespace AudioEngineersPlatformBackend.Contracts.Auth.Login;

public record LoginRequest(
    string Email,
    string Password
);