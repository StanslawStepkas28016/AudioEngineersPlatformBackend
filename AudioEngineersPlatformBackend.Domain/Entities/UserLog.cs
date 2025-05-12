using System.Security.Cryptography;

namespace AudioEngineersPlatformBackend.Domain.Entities;

public enum VerificationOutcome
{
    Success,
    VerificationCodeExpired,
}

public class UserLog
{
    public Guid IdUserLog { get; private set; }
    public DateTime? DateCreated { get; private set; }
    public DateTime? DateDeleted { get; private set; }
    public bool IsDeleted { get; private set; }
    public string? VerificationCode { get; private set; }
    public DateTime? VerificationCodeExpiration { get; private set; }
    public bool IsVerified { get; private set; }
    public DateTime? DateLastLogin { get; private set; }
    public ICollection<User> Users { get; private set; }

    public UserLog()
    {
        IdUserLog = Guid.NewGuid();
        DateCreated = DateTime.UtcNow;
        DateDeleted = null;
        IsDeleted = false;
        VerificationCode = RandomNumberGenerator.GetInt32(0, 1000000).ToString("D6");
        VerificationCodeExpiration = DateTime.UtcNow.AddHours(24);
        IsVerified = false;
        DateLastLogin = null;
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

    public void SetDateLastLogin()
    {
        DateLastLogin = DateTime.UtcNow;
    }
}