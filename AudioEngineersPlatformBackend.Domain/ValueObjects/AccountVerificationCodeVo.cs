namespace AudioEngineersPlatformBackend.Domain.ValueObjects;

public readonly struct AccountVerificationCodeVo
{
    private readonly string _verificationCode;

    public string VerificationCode
    {
        get => _verificationCode;
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

    public AccountVerificationCodeVo(string verificationCode)
    {
        VerificationCode = verificationCode;
    }
}