using System.Security.Cryptography;

namespace AudioEngineersPlatformBackend.Domain.ValueObjects;

public readonly struct VerificationCodeVo
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

    public static string Generate()
    {
        return RandomNumberGenerator
            .GetInt32(0, 1000000)
            .ToString("D6");
    }

    public VerificationCodeVo(
        string verificationCode
    )
    {
        VerificationCode = verificationCode;
    }
}