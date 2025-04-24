namespace AudioEngineersPlatformBackend.Dtos.Auth.Utilities;

public class VerifyEmailUserDataDto
{
    public string Email { get; set; }
    public string Username { get; set; }
    public string VerificationCode { get; set; }
}