using System.Security.Cryptography;

namespace AudioEngineersPlatformBackend.Domain.Entities;

public enum VerificationOutcome
{
    Success,
    VerificationCodeExpired,
}

public class UserLog
{
    // Backing fields
    private Guid _idUserLog;
    private DateTime? _dateCreated;
    private DateTime? _dateDeleted;
    private bool _isDeleted;
    private string? _verificationCode;
    private DateTime? _verificationCodeExpiration;
    private bool _isVerified;
    private DateTime? _dateLastLogin;
    private string? _refreshToken;
    private DateTime? _refreshTokenExp;
    private ICollection<User> _users;

    // Properties
    public Guid IdUserLog
    {
        get { return _idUserLog; }
        private set
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException("IdUserLog cannot be empty", nameof(value));
            }

            _idUserLog = value;
        }
    }

    public DateTime? DateCreated
    {
        get { return _dateCreated; }
        private set
        {
            if (value == null)
            {
                throw new ArgumentException("DateCreated cannot be null", nameof(value));
            }

            _dateCreated = value;
        }
    }

    public DateTime? DateDeleted
    {
        get { return _dateDeleted; }
        private set { _dateDeleted = value; }
    }

    public bool IsDeleted
    {
        get { return _isDeleted; }
        private set { _isDeleted = value; }
    }

    public string? VerificationCode
    {
        get { return _verificationCode; }
        private set { _verificationCode = value; }
    }

    public DateTime? VerificationCodeExpiration
    {
        get { return _verificationCodeExpiration; }
        private set { _verificationCodeExpiration = value; }
    }

    public bool IsVerified
    {
        get { return _isVerified; }
        private set { _isVerified = value; }
    }

    public DateTime? DateLastLogin
    {
        get { return _dateLastLogin; }
        private set { _dateLastLogin = value; }
    }

    public string? RefreshToken
    {
        get { return _refreshToken; }
        private set { _refreshToken = value; }
    }

    public DateTime? RefreshTokenExp
    {
        get { return _refreshTokenExp; }
        private set { _refreshTokenExp = value; }
    }

    // References
    public ICollection<User> Users
    {
        get { return _users; }
        private set { _users = value; }
    }

    // Private constructor used for EF Core
    private UserLog()
    {
    }
    
    /// <summary>
    ///     Method used for verifying the user account. It is being invoked when the user
    ///     provides the correct verification code that was sent to their email.
    /// </summary>
    /// <returns></returns>
    public VerificationOutcome VerifyUserAccount()
    {
        if (VerificationCodeExpiration < DateTime.UtcNow)
        {
            IsDeleted = true;
            DateDeleted = DateTime.UtcNow;
            VerificationCode = null;
            VerificationCodeExpiration = null;
            return VerificationOutcome.VerificationCodeExpired;
        }

        IsVerified = true;
        VerificationCode = null;
        VerificationCodeExpiration = null;

        return VerificationOutcome.Success;
    }

    /// <summary>
    ///     Method used for checking if the user is deleted or not verified.
    ///     As the Try... suggests this method will throw an exception based on the user status.
    /// </summary>
    /// <exception cref="Exception"></exception>
    public void TryCheckUserStatus()
    {
        if (IsDeleted)
        {
            throw new Exception("User is deleted");
        }

        if (!IsVerified)
        {
            throw new Exception("User is not verified");
        }
    }

    /// <summary>
    ///     Method used for setting login associated data.
    ///     This method should be called when the user provides correct credentials.
    /// </summary>
    /// <param name="refreshToken"></param>
    /// <param name="refreshTokenExp"></param>
    /// <exception cref="ArgumentException"></exception>
    public void SetLoginData(string refreshToken, DateTime refreshTokenExp)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            throw new ArgumentException("Refresh token cannot be empty", nameof(refreshToken));
        }

        if (refreshTokenExp <= DateTime.UtcNow)
        {
            throw new ArgumentException("Refresh token expiration date must be in the future", nameof(refreshTokenExp));
        }

        RefreshToken = refreshToken;
        RefreshTokenExp = refreshTokenExp;
        DateLastLogin = DateTime.UtcNow;
    }

    /// <summary>
    ///     Factory method for creating a new UserLog instance.
    ///     This method initializes the UserLog with default values.
    ///     It is there because EF.Core requires a parameterless constructor for entity classes,
    ///     thus it is not possible to utilize a parameterless constructor for UserLog creation.
    /// </summary>
    /// <returns></returns>
    public static UserLog Create()
    {
        return new UserLog
        {
            IdUserLog = Guid.NewGuid(),
            DateCreated = DateTime.UtcNow,
            DateDeleted = null,
            IsDeleted = false,
            VerificationCode = RandomNumberGenerator.GetInt32(0, 1000000).ToString("D6"),
            VerificationCodeExpiration = DateTime.UtcNow.AddHours(24),
            IsVerified = false,
            DateLastLogin = null,
            RefreshToken = null,
            RefreshTokenExp = null,
        };
    }

    /// <summary>
    ///     Factory method for creating a new UserLog with a provided idUserLog.
    ///     Used for seeding.
    /// </summary>
    /// <param name="idUserLog"></param>
    /// <returns></returns>
    public static UserLog CreateWithId(Guid idUserLog)
    {
        return new UserLog
        {
            IdUserLog = idUserLog,
            DateCreated = DateTime.UtcNow,
            DateDeleted = null,
            IsDeleted = false,
            VerificationCode = RandomNumberGenerator.GetInt32(0, 1000000).ToString("D6"),
            VerificationCodeExpiration = DateTime.UtcNow.AddHours(24),
            IsVerified = false,
            DateLastLogin = null,
            RefreshToken = null,
            RefreshTokenExp = null,
        };
    }
}