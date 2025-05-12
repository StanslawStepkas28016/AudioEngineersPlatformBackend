namespace AudioEngineersPlatformBackend.Domain.ValueObjects;

public class VerificationCodeVO
{
    private string _verificationCode { get; set; }

    public VerificationCodeVO(string verificationCode)
    {
        if (string.IsNullOrWhiteSpace(verificationCode))
        {
            throw new ArgumentException("Verification code cannot be null", nameof(verificationCode));
        }

        if (verificationCode.Length != 6)
        {
            throw new ArgumentException("Verification code must be 6 characters long", nameof(verificationCode));
        }

        _verificationCode = verificationCode;
    }

    public string GetValidVerificationCode()
    {
        return _verificationCode;
    }
}