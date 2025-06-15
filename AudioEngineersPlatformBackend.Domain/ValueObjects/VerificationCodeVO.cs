namespace AudioEngineersPlatformBackend.Domain.ValueObjects;

public class VerificationCodeVo
{
    private string VerificationCode { get; set; }

    public VerificationCodeVo(string verificationCode)
    {
        if (string.IsNullOrWhiteSpace(verificationCode))
        {
            throw new ArgumentException("Verification code cannot be null", nameof(verificationCode));
        }

        if (verificationCode.Length != 6)
        {
            throw new ArgumentException("Verification code must be 6 characters long", nameof(verificationCode));
        }

        VerificationCode = verificationCode;
    }

    public string GetValidVerificationCode()
    {
        return VerificationCode;
    }
}