using AudioEngineersPlatformBackend.Domain.Exceptions;

namespace AudioEngineersPlatformBackend.Domain.Entities;

public class AdvertLog
{
    // Backing fields
    private Guid _idAdvertLog;

    // Properties
    public Guid IdAdvertLog
    {
        get => _idAdvertLog;
        private set
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException($"{nameof(IdAdvertLog)} cannot be empty.");
            }

            _idAdvertLog = value;
        }
    }

    public DateTime DateCreated { get; private set; }

    public DateTime? DateModified { get; private set; }

    public DateTime? DateDeleted { get; private set; }

    public bool IsDeleted { get; private set; }

    public bool IsActive { get; private set; }

    // References
    public ICollection<Advert> Adverts { get; set; }

    // Private constructor for EF Core
    private AdvertLog()
    {
    }

    public static AdvertLog Create()
    {
        return new AdvertLog
        {
            IdAdvertLog = Guid.NewGuid(),
            DateCreated = DateTime.UtcNow,
            DateModified = null,
            DateDeleted = null,
            IsDeleted = false,
            IsActive = true
        };
    }

    public static AdvertLog CreateWithIdAndStaticData(
        Guid idAdvertLog,
        DateTime dateCreated
    )
    {
        return new AdvertLog
        {
            IdAdvertLog = idAdvertLog,
            DateCreated = dateCreated,
            DateModified = null,
            DateDeleted = null,
            IsDeleted = false,
            IsActive = true
        };
    }

    public void SetIsDeletedStatus(
        bool isDeleted
    )
    {
        IsDeleted = isDeleted;
        DateDeleted = DateTime.UtcNow;
    }

    public void SetIsActiveStatus(
        bool isActive
    )
    {
        IsActive = isActive;
    }

    public void EnsureCorrectStatus()
    {
        if (!IsActive || IsDeleted)
        {
            throw new BusinessRelatedException("Advert not available.");
        }
    }
}