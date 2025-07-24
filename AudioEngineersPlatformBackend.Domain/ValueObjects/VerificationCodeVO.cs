namespace AudioEngineersPlatformBackend.Domain.ValueObjects;

public readonly struct VerificationCodeVo
{
    private readonly string? _verificationCode;

    private string VerificationCode
    {
        get { return _verificationCode; }
        init
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Verification code cannot be null", nameof(value));
            }

            if (value.Length != 6)
            {
                throw new ArgumentException("Verification code must be 6 characters long", nameof(value));
            }

            _verificationCode = value;
        }
    }

    public VerificationCodeVo(string verificationCode)
    {
        VerificationCode = verificationCode;
    }

    public string GetValidVerificationCode()
    {
        return VerificationCode;
    }
}