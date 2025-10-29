namespace AudioEngineersPlatformBackend.Domain.Entities;

public enum TokenNames
{
    VerifyAccountToken,
    RefreshToken,
    ForgotPasswordToken,
    ResetEmailToken,
    ResetPasswordToken
}

public class Token
{
    // Backing fields
    private Guid _idToken;
    private string _name;
    private string _value;

    // Constants
    public static DateTime RefreshTokenExpirationDate => DateTime.UtcNow.AddHours(24);
    public static DateTime VerifyAccountTokenExpirationDate => DateTime.UtcNow.AddHours(1);
    public static DateTime ForgotPasswordTokenExpirationDate => DateTime.UtcNow.AddHours(1);
    public static DateTime ResetEmailTokenExpirationDate => DateTime.UtcNow.AddHours(1);
    public static DateTime ResetPasswordTokenExpirationDate => DateTime.UtcNow.AddHours(1);

    // Properties
    public Guid IdToken
    {
        get => _idToken;
        private set
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException($"{nameof(IdToken)} cannot be empty.");
            }

            _idToken = value;
        }
    }

    public string Name
    {
        get => _name;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"{nameof(Name)} cannot be empty.");
            }

            _name = value;
        }
    }

    public string Value
    {
        get => _value;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"{nameof(Value)} cannot be empty.");
            }

            _value = value;
        }
    }

    public DateTime ExpirationDate { get; private set; }

    // References
    public Guid IdUser { get; set; }
    public User User { get; set; }

    // Private constructor used for EF Core
    private Token()
    {
    }

    // Factory methods
    public static Token Create(
        string tokenName,
        string value,
        DateTime tokenExpiration,
        Guid idUser
    )
    {
        return new Token
        {
            IdToken = Guid.NewGuid(),
            Name = tokenName,
            Value = value,
            ExpirationDate = tokenExpiration,
            IdUser = idUser
        };
    }

    public static Token CreateWithId(
        Guid idUserToken,
        string tokenName,
        string value,
        DateTime tokenExpiration,
        Guid idUser
    )
    {
        return new Token
        {
            IdToken = idUserToken,
            Name = tokenName,
            Value = value,
            ExpirationDate = tokenExpiration,
            IdUser = idUser
        };
    }

    public bool TryVerify()
    {
        if (ExpirationDate < DateTime.UtcNow)
        {
            return false;
        }

        return true;
    }

    public void UpdateTokenData(
        string newValue,
        DateTime newExpirationDate
    )
    {
        Value = newValue;
        ExpirationDate = newExpirationDate;
    }
}