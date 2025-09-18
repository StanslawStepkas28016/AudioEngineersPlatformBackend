using System.Security.Cryptography;

namespace AudioEngineersPlatformBackend.Domain.Entities;

public enum VerificationOutcome
{
    Success,
    VerificationCodeExpired,
}

public partial class UserAuthLog
{
    // Properties
    public Guid IdUserAuthLog
    {
        get => _idUserAuthLog;
        private set
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException($"{nameof(IdUserAuthLog)} cannot be empty.");
            }

            _idUserAuthLog = value;
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

    public Guid ForgotPasswordToken
    {
        get => _forgotPasswordToken;
        set
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException($"{nameof(ForgotPasswordToken)} cannot be empty.");
            }

            _forgotPasswordToken = value;
        }
    }

    public DateTime ForgotPasswordTokenExpiration
    {
        get => _forgotPasswordTokenExpiration;
        set
        {
            if (value < DateTime.UtcNow)
            {
                throw new ArgumentException($"{nameof(ForgotPasswordTokenExpiration)} cannot be in past.");
            }
            
            _forgotPasswordTokenExpiration = value;
        }
    }

    public bool IsRemindingPassword
    {
        get => _isRemindingPassword;
        set => _isRemindingPassword = value;
    }

    // References
    public ICollection<User> Users
    {
        get => _users;
        private set => _users = value;
    }

    // Private constructor used for EF Core
    private UserAuthLog()
    {
    }

    /// <summary>
    ///     Factory method for creating a new UserAuthLog instance.
    ///     This method initializes the UserAuthLog with default values.
    ///     It is there because EF.Core requires a parameterless constructor for entity classes,
    ///     thus it is not possible to utilize a parameterless constructor for UserAuthLog creation.
    /// </summary>
    /// <returns></returns>
    public static UserAuthLog Create()
    {
        return new UserAuthLog
        {
            IdUserAuthLog = Guid.NewGuid(),
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
    ///     Factory method for creating a new UserAuthLog with a provided idUserLog.
    ///     Used for seeding.
    /// </summary>
    /// <param name="idUserLog"></param>
    /// <param name="dateCreated"></param>
    /// <returns></returns>
    public static UserAuthLog CreateWithIdAndStaticData(Guid idUserLog, DateTime dateCreated)
    {
        return new UserAuthLog
        {
            IdUserAuthLog = idUserLog,
            DateCreated = dateCreated,
            DateDeleted = null,
            IsDeleted = false,
            RefreshToken = null,
            RefreshTokenExpiration = null,
            DateLastLogin = null,
            VerificationCode = null,
            VerificationCodeExpiration = null,
            IsVerified = true,
            ResetEmailToken = null,
            ResetEmailTokenExpiration = null,
            IsResettingEmail = false,
            ResetPasswordToken = null,
            ResetPasswordTokenExpiration = null,
            IsResettingPassword = false
        };
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
    /// </summary>
    /// <exception cref="Exception"></exception>
    public void EnsureCorrectUserStatus()
    {
        if (IsDeleted)
        {
            throw new Exception($"{nameof(User)} is deleted.");
        }

        if (!IsVerified)
        {
            throw new Exception($"{nameof(User)} is not verified.");
        }

        if (IsResettingEmail)
        {
            throw new Exception($"{nameof(User)} is resetting their email address.");
        }

        if (IsResettingPassword)
        {
            throw new Exception($"{nameof(User)} is resetting their password.");
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
    ///     This method will probably have to be refactored if the app grows.
    /// </summary>
    /// <returns></returns>
    private static string GenerateVerificationCode()
    {
        return RandomNumberGenerator.GetInt32(0, 1000000).ToString("D6");
    }

    /// <summary>
    ///     Method used for persisting information regarding
    ///     email resetting.
    /// </summary>
    /// <param name="resetEmailToken"></param>
    public void SetResetEmailData(Guid resetEmailToken)
    {
        if (resetEmailToken == Guid.Empty)
        {
            throw new ArgumentException($"{nameof(resetEmailToken)} cannot be empty.");
        }

        ResetEmailToken = resetEmailToken;
        ResetEmailTokenExpiration = DateTime.UtcNow.AddHours(1);
        IsResettingEmail = true;
    }

    /// <summary>
    ///     Method used for verifying the reset email related data.
    ///     It ensures that the token is correct and valid, as well as
    ///     setting the data reset email data to the correct state
    ///     provided the verification is successful.
    /// </summary>
    /// <param name="resetEmailTokenValidated"></param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="Exception"></exception>
    public void VerifyResetEmailData(Guid resetEmailTokenValidated)
    {
        if (!IsResettingEmail)
        {
            throw new ArgumentException($"{nameof(User)} is not marked as resetting email.");
        }

        if (resetEmailTokenValidated != ResetEmailToken)
        {
            throw new ArgumentException($"{nameof(resetEmailTokenValidated)} is not valid.");
        }

        if (ResetEmailTokenExpiration < DateTime.UtcNow)
        {
            throw new Exception($"The {nameof(ResetEmailToken)} has expired.");
        }

        ResetEmailToken = null;
        ResetEmailTokenExpiration = null;
        IsResettingEmail = false;
    }

    public void SetResetPasswordData(Guid resetPasswordToken)
    {
        if (resetPasswordToken == Guid.Empty)
        {
            throw new Exception($"{nameof(resetPasswordToken)} cannot be empty.");
        }

        ResetPasswordToken = resetPasswordToken;
        ResetPasswordTokenExpiration = DateTime.UtcNow.AddHours(1);
        IsResettingPassword = true;
    }

    public void VerifyResetPasswordData(Guid resetPasswordTokenValidated)
    {
        if (!IsResettingPassword)
        {
            throw new ArgumentException($"{nameof(User)} is not marked as resetting password.");
        }

        if (resetPasswordTokenValidated != ResetPasswordToken)
        {
            throw new ArgumentException($"{nameof(resetPasswordTokenValidated)} is not valid.");
        }

        if (ResetPasswordTokenExpiration < DateTime.UtcNow)
        {
            throw new Exception($"The {nameof(ResetPasswordTokenExpiration)} has expired.");
        }


        ResetPasswordToken = null;
        ResetPasswordTokenExpiration = null;
        IsResettingPassword = false;
    }

    public void SetForgotPasswordData(Guid forgotPasswordToken)
    {
        if (forgotPasswordToken == Guid.Empty)
        {
            throw new ArgumentException($"{nameof(forgotPasswordToken)} cannot be empty.");
        }

        ForgotPasswordToken = forgotPasswordToken;
        ForgotPasswordTokenExpiration = DateTime.UtcNow.AddHours(1);
        IsRemindingPassword = true;
    }

    public void VerifyForgotPasswordData(Guid forgotPasswordTokenValidated)
    {
        throw new NotImplementedException();
    }
}

public partial class UserAuthLog
{
    // Backing fields
    private Guid _idUserAuthLog;
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
    private Guid _forgotPasswordToken;
    private DateTime _forgotPasswordTokenExpiration;
    private bool _isRemindingPassword;
    private ICollection<User> _users;
}