namespace AudioEngineersPlatformBackend.Contracts.Auth.ResetPassword;

public record ResetPasswordRequest
(
    string CurrentPassword,
    string NewPassword,
    string NewPasswordRepeated
);