namespace AudioEngineersPlatformBackend.Contracts.Auth.VerifyForgotPassword;

public class VerifyForgotPasswordRequest
{
    public string NewPassword { get; set; } = string.Empty;
    public string NewPasswordRepeated { get; set; } = string.Empty;
}