using System.Security.Cryptography;

namespace AudioEngineersPlatformBackend.Domain.Entities;

public enum VerificationOutcome
{
    Success,
    VerificationCodeExpired,
}

public class UserLog
{
    // Properties
    public Guid IdUserLog { get; private set; }
    public DateTime? DateCreated { get; private set; }
    public DateTime? DateDeleted { get; private set; }
    public bool IsDeleted { get; private set; }
    public string? VerificationCode { get; private set; }
    public DateTime? VerificationCodeExpiration { get; private set; }
    public bool IsVerified { get; private set; }
    public DateTime? DateLastLogin { get; private set; }
    public string? RefreshToken { get; private set; }
    public DateTime? RefreshTokenExp { get; private set; }

    // References (Navigation Properties)
    public ICollection<User> Users { get; private set; }

    private UserLog()
    {
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
    ///     This method should be called after successful login.
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
}