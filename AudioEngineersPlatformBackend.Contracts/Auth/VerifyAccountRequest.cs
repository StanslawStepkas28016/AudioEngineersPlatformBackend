namespace AudioEngineersPlatformBackend.Contracts.Auth;

public record VerifyAccountRequest(
    Guid IdUser,
    string VerificationCode
);