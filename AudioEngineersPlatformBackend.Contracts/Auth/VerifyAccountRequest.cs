namespace AudioEngineersPlatformBackend.Contracts.Auth;

public record VerifyAccountRequest(
    string VerificationCode
);