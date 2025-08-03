namespace AudioEngineersPlatformBackend.Domain.Entities;

public class AdvertLog
{
    // Backing fields
    private Guid _idAdvertLog;
    private DateTime _dateCreated;
    private DateTime? _dateModified;
    private DateTime? _dateDeleted;
    private bool _isDeleted;
    private bool _isActive;
    private ICollection<Advert> _adverts;

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

    public DateTime DateCreated
    {
        get => _dateCreated;
        private set => _dateCreated = value;
    }

    public DateTime? DateModified
    {
        get => _dateModified;
        private set => _dateModified = value;
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

    public bool IsActive
    {
        get => _isActive;
        private set => _isActive = value;
    }

    // References
    public ICollection<Advert> Adverts
    {
        get => _adverts;
        set => _adverts = value;
    }
    
    // Methods
    public void MarkAsDeleted()
    {
        if (IsDeleted || !IsActive || DateDeleted.HasValue)
        {
            throw new ArgumentException($"{GetType()} is already deleted.");
        }

        IsDeleted = true;
        DateDeleted = DateTime.UtcNow;
        IsActive = false;
    }

    public void UndoMarkAsDeleted()
    {
        IsDeleted = false;
        DateDeleted = null;
        IsActive = true;
    }

    // Private constructor for EF Core
    private AdvertLog()
    {
    }

    /// <summary>
    ///     Factory method for creating a new AdvertLog instance.
    ///     As in the case of UserLog, this method initializes the AdvertLog with default values.
    ///     It is there because EF.Core requires a parameterless constructor for entity classes,
    ///     thus it is not possible to utilize a parameterless constructor for AdvertLog creation.
    /// </summary>
    /// <returns></returns>
    public static AdvertLog Create()
    {
        return new AdvertLog
        {
            IdAdvertLog = Guid.NewGuid(),
            DateCreated = DateTime.UtcNow,
            DateModified = null,
            DateDeleted = null,
            IsDeleted = false,
            IsActive = true,
        };
    }

    /// <summary>
    ///     Method for creating a new AdvertLog with a provided idAdvertLog.
    ///     Used for seeding data.
    /// </summary>
    /// <param name="idAdvertLog"></param>
    /// <returns></returns>
    public static AdvertLog CreateWithId(Guid idAdvertLog)
    {
        return new AdvertLog
        {
            IdAdvertLog = idAdvertLog,
            DateCreated = DateTime.UtcNow,
            DateModified = null,
            DateDeleted = null,
            IsDeleted = false,
            IsActive = true,
        };
    }
}