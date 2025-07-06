namespace AudioEngineersPlatformBackend.Contracts.Auth.Register;

public record RegisterResponse(
    Guid IdUser,
    string Email
);