namespace AudioEngineersPlatformBackend.Domain.Entities;

public class AdvertCategory
{
    // Backing fields
    private Guid _idAdvertCategory;
    private string _categoryName;
    private ICollection<Advert> _adverts;

    // Properties
    public Guid IdAdvertCategory
    {
        get { return _idAdvertCategory; }
        private set
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException("IdAdvertCategory cannot be empty.", nameof(value));
            }

            _idAdvertCategory = value;
        }
    }

    public string CategoryName
    {
        get { return _categoryName; }
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("CategoryName cannot be null or whitespace.", nameof(value));
            }

            _categoryName = value;
        }
    }

    // References (Navigation Properties)
    public ICollection<Advert> Adverts
    {
        get { return _adverts; }
        set { _adverts = value; }
    }

    // Factory method to create a new AdvertCategory
    public static AdvertCategory Create(string categoryName)
    {
        return new AdvertCategory
        {
            IdAdvertCategory = Guid.NewGuid(),
            CategoryName = categoryName,
        };
    }

    /// <summary>
    ///     Method to set the IdAdvertCategory for seeding purposes ONLY.
    /// </summary>
    /// <param name="idAdvertCategory"></param>
    /// <exception cref="ArgumentException"></exception>
    public void SetIdAdvertCategoryForSeeding(Guid idAdvertCategory)
    {
        if (idAdvertCategory == Guid.Empty)
        {
            throw new ArgumentException("IdAdvertCategory cannot be empty.", nameof(idAdvertCategory));
        }

        IdAdvertCategory = idAdvertCategory;
    }
}