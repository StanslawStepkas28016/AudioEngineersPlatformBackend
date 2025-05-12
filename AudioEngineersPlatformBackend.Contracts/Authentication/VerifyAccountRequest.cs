namespace AudioEngineersPlatformBackend.Contracts.Authentication;

public record VerifyAccountRequest(
    Guid IdUser,
    string VerificationCode
);