namespace AudioEngineersPlatformBackend.Domain.ValueObjects;

public readonly struct AccountVerificationCodeVO
{
    private readonly string? _verificationCode;

    private string VerificationCode
    {
        get { return _verificationCode; }
        init
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"{nameof(VerificationCode)} cannot be null.");
            }

            if (value.Length != 6)
            {
                throw new ArgumentException($"{nameof(VerificationCode)} must be 6 characters long.");
            }

            _verificationCode = value;
        }
    }

    public AccountVerificationCodeVO(string verificationCode)
    {
        VerificationCode = verificationCode;
    }

    public string GetValidAccountVerificationCode()
    {
        return VerificationCode;
    }
}