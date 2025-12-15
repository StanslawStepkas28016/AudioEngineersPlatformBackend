using AudioEngineersPlatformBackend.Domain.Exceptions;

namespace AudioEngineersPlatformBackend.Domain.Entities;

public class UserAuthLog
{
    // Backing fields
    private Guid _idUserAuthLog;
    private DateTime? _dateCreated;

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

    public DateTime? DateDeleted { get; private set; }
    public DateTime? DateLastLogin { get; private set; }
    public bool IsDeleted { get; private set; }
    public bool IsVerified { get; private set; }
    public bool IsResettingEmail { get; set; }
    public bool IsResettingPassword { get; set; }
    public bool IsRemindingPassword { get; set; }

    // References
    public ICollection<User> Users { get; private set; }

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
            DateLastLogin = null,
            DateDeleted = null,
            IsDeleted = false,
            IsVerified = false,
            IsResettingEmail = false,
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
    public static UserAuthLog CreateWithIdAndStaticData(
        Guid idUserLog,
        DateTime dateCreated
    )
    {
        return new UserAuthLog
        {
            IdUserAuthLog = idUserLog,
            DateCreated = dateCreated,
            DateLastLogin = null,
            DateDeleted = null,
            IsDeleted = false,
            IsVerified = false,
            IsResettingEmail = false,
            IsResettingPassword = false
        };
    }

    public void SetIsVerifiedStatus(
        bool isVerified
    )
    {
        IsVerified = isVerified;
    }

    public void SetIsDeletedStatusAndData(
        bool isDeleted
    )
    {
        IsDeleted = isDeleted;
        DateDeleted = DateTime.UtcNow;
    }

    public void SetIsRemindingPasswordStatus(
        bool isRemindingPassword
    )
    {
        IsRemindingPassword = isRemindingPassword;
    }

    public void SetIsResettingEmail(
        bool isResettingEmail
    )
    {
        IsResettingEmail = isResettingEmail;
    }

    public void EnsureCorrectUserStatus()
    {
        if (IsDeleted)
        {
            throw new BusinessRelatedException($"{nameof(User)} is deleted.");
        }

        if (!IsVerified)
        {
            throw new BusinessRelatedException($"{nameof(User)} is not verified.");
        }

        if (IsResettingEmail)
        {
            throw new BusinessRelatedException($"{nameof(User)} is resetting their email address.");
        }

        if (IsResettingPassword)
        {
            throw new BusinessRelatedException($"{nameof(User)} is resetting their password.");
        }
    }

    public void SetLoginData()
    {
        DateLastLogin = DateTime.UtcNow;
    }

    public void SetIsResettingPassword(
        bool isResettingPassword
    )
    {
        IsResettingPassword = isResettingPassword;
    }
}