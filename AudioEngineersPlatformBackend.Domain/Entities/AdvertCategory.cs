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
        get => _idAdvertCategory;
        private set
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException($"{nameof(IdAdvertCategory)} cannot be empty.");
            }

            _idAdvertCategory = value;
        }
    }

    public string CategoryName
    {
        get => _categoryName;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"{nameof(CategoryName)} cannot be null or whitespace.");
            }

            _categoryName = value;
        }
    }

    // References
    public ICollection<Advert> Adverts
    {
        get => _adverts;
        set => _adverts = value;
    }

    /// <summary>
    ///     Factory method to create a new AdvertCategory.
    /// </summary>
    /// <param name="categoryName"></param>
    /// <returns></returns>
    public static AdvertCategory Create(string categoryName)
    {
        return new AdvertCategory
        {
            IdAdvertCategory = Guid.NewGuid(),
            CategoryName = categoryName,
        };
    }

    /// <summary>
    ///     Factory method to create a new AdvertCategory with specific IdAdvertCategory.
    ///     Used for seeding.
    /// </summary>
    /// <param name="idAdvertCategory"></param>
    /// <param name="categoryName"></param>
    /// <returns></returns>
    public static AdvertCategory CreateWithId(Guid idAdvertCategory, string categoryName)
    {
        return new AdvertCategory
        {
            IdAdvertCategory = idAdvertCategory,
            CategoryName = categoryName,
        };
    }
}