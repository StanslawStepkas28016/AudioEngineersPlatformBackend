namespace AudioEngineersPlatformBackend.Contracts.Auth.ResetPassword;

public record ResetPasswordRequest
(
    string OldPassword,
    string NewPassword
);