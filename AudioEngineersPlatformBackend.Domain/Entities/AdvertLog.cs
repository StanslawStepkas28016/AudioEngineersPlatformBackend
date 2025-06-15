namespace AudioEngineersPlatformBackend.Domain.Entities;

public class AdvertLog
{
    public Guid IdAdvertLog { get; private set; }
    public DateTime DateCreated { get; private set; }
    public DateTime? DateModified { get; private set; }
    public DateTime? DateDeleted { get; private set; }
    public bool IsDeleted { get; private set; }
    public bool IsActive { get; private set; }
    public ICollection<Advert> Adverts { get; set; }

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
}