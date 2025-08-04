namespace AudioEngineersPlatformBackend.Contracts.Auth.VerifyResetEmail;

public record VerifyResetEmailResponse(
    Guid IdUser,
    bool IsResettingEmail
);