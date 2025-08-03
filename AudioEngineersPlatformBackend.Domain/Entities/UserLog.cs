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
    private string? _refreshToken;
    private DateTime? _refreshTokenExpiration;
    private DateTime? _dateLastLogin;
    private string? _verificationCode;
    private DateTime? _verificationCodeExpiration;
    private bool _isVerified;
    private Guid? _resetPasswordToken;
    private DateTime? _resetPasswordTokenExpiration;
    private bool _isResettingPassword;
    private Guid? _resetEmailToken;
    private DateTime? _resetEmailTokenExpiration;
    private bool _isResettingEmail;
    private ICollection<User> _users;

    // Properties
    public Guid IdUserLog
    {
        get => _idUserLog;
        private set
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException($"{nameof(IdUserLog)} cannot be empty.");
            }

            _idUserLog = value;
        }
    }

    public DateTime? DateCreated
    {
        get => _dateCreated;
        private set
        {
            if (value == null)
            {
                throw new ArgumentException($"{nameof(DateCreated)} cannot be null.");
            }

            _dateCreated = value;
        }
    }

    public DateTime? DateDeleted
    {
        get => _dateDeleted;
        private set => _dateDeleted = value;
    }

    public bool IsDeleted
    {
        get => _isDeleted;
        private set => _isDeleted = value;
    }
    
    public string? RefreshToken
    {
        get => _refreshToken;
        private set => _refreshToken = value;
    }

    public DateTime? RefreshTokenExpiration
    {
        get => _refreshTokenExpiration;
        private set => _refreshTokenExpiration = value;
    }
    
    public DateTime? DateLastLogin
    {
        get => _dateLastLogin;
        private set => _dateLastLogin = value;
    }

    public string? VerificationCode
    {
        get => _verificationCode;
        private set => _verificationCode = value;
    }

    public DateTime? VerificationCodeExpiration
    {
        get => _verificationCodeExpiration;
        private set => _verificationCodeExpiration = value;
    }

    public bool IsVerified
    {
        get => _isVerified;
        private set => _isVerified = value;
    }
    
    public Guid? ResetEmailToken
    {
        get { return _resetEmailToken; }
        private set { _resetEmailToken = value; }
    }

    public DateTime? ResetEmailTokenExpiration
    {
        get { return _resetEmailTokenExpiration; }
        private set { _resetEmailTokenExpiration = value; }
    }

    public bool IsResettingEmail
    {
        get { return _isResettingEmail; }
        set { _isResettingEmail = value; }
    }
    
    public Guid? ResetPasswordToken
    {
        get { return _resetPasswordToken; }
        private set { _resetPasswordToken = value; }
    }

    public DateTime? ResetPasswordTokenExpiration
    {
        get { return _resetPasswordTokenExpiration; }
        private set { _resetPasswordTokenExpiration = value; }
    }

    public bool IsResettingPassword
    {
        get { return _isResettingPassword; }
        set { _isResettingPassword = value; }
    }
    
    // References
    public ICollection<User> Users
    {
        get => _users;
        private set => _users = value;
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
            throw new Exception("User is deleted.");
        }

        if (!IsVerified)
        {
            throw new Exception("User is not verified.");
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
            throw new ArgumentException($"{nameof(refreshToken)} cannot be empty.");
        }

        if (refreshTokenExp <= DateTime.UtcNow)
        {
            throw new ArgumentException($"{nameof(refreshTokenExp)} must be in the future.");
        }

        RefreshToken = refreshToken;
        RefreshTokenExpiration = refreshTokenExp;
        DateLastLogin = DateTime.UtcNow;
    }

    /// <summary>
    ///     Method user for logging the user out of all their sessions.
    ///     Should only be used when performing operations like resetting emails, password etc.
    /// </summary>
    public void SetLogoutData()
    {
        RefreshToken = "";
        RefreshTokenExpiration = null;
    }

    /// <summary>
    ///     Method used for generating a 6 digit verification code.
    ///     This code does not need to be unique for now, as it is going to be
    ///     deleted soon after its issuing time - 24 hours for now.
    ///     This method will probably have to be refactored if the app grows
    /// </summary>
    /// <returns></returns>
    private static string GenerateVerificationCode()
    {
        return RandomNumberGenerator.GetInt32(0, 1000000).ToString("D6");
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
            RefreshToken = null,
            RefreshTokenExpiration = null,
            DateLastLogin = null,
            VerificationCode = GenerateVerificationCode(),
            VerificationCodeExpiration = DateTime.UtcNow.AddHours(24),
            IsVerified = false,
            ResetEmailToken = null,
            ResetEmailTokenExpiration = null,
            IsResettingEmail = false,
            ResetPasswordToken = null,
            ResetPasswordTokenExpiration = null,
            IsResettingPassword = false
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
            RefreshToken = null,
            RefreshTokenExpiration = null,
            DateLastLogin = null,
            VerificationCode = GenerateVerificationCode(),
            VerificationCodeExpiration = DateTime.UtcNow.AddHours(24),
            IsVerified = false,
            ResetEmailToken = null,
            ResetEmailTokenExpiration = null,
            IsResettingEmail = false,
            ResetPasswordToken = null,
            ResetPasswordTokenExpiration = null,
            IsResettingPassword = false
        };
    }
}