namespace AudioEngineersPlatformBackend.Contracts.Auth.VerifyAccount;

public record VerifyAccountRequest(
    string VerificationCode
);