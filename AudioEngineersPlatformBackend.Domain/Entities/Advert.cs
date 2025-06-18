namespace AudioEngineersPlatformBackend.Domain.Entities;

public class Advert
{
    // Backing fields
    private Guid _idAdvert;
    private string _title;
    private string _description;
    private Guid _coverImageKey;
    private string _portfolioUrl;
    private double _price;
    private Guid _idUser;
    private Guid _idAdvertCategory;
    private Guid _idAdvertLog;
    private User _user;
    private AdvertCategory _advertCategory;
    private AdvertLog _advertLog;

    // Constants
    private const double MaxPrice = 1500.0;
    private const int MaxTitleLength = 100;
    private const int MaxDescriptionLength = 1500;

    // Properties
    public Guid IdAdvert
    {
        get => _idAdvert;
        private set
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException($"{nameof(IdAdvert)} cannot be empty.");
            }

            _idAdvert = value;
        }
    }

    public string Title
    {
        get => _title;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"{nameof(Title)} cannot be empty.");
            }


            if (value.Length > MaxTitleLength)
            {
                throw new ArgumentException($"{nameof(Title)} cannot exceed {MaxTitleLength} characters.");
            }

            _title = value;
        }
    }

    public string Description
    {
        get => _description;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"{nameof(Description)} cannot be null or whitespace.");
            }

            if (value.Length > MaxDescriptionLength)
            {
                throw new ArgumentException($"{nameof(Description)} cannot exceed {MaxDescriptionLength} characters.",
                    nameof(value));
            }

            _description = value;
        }
    }

    public Guid CoverImageKey
    {
        get => _coverImageKey;
        private set
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException($"{nameof(CoverImageKey)} cannot be null or whitespace.");
            }

            _coverImageKey = value;
        }
    }

    public string PortfolioUrl
    {
        get => _portfolioUrl;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"{nameof(PortfolioUrl)} cannot be null or whitespace.");
            }

            _portfolioUrl = value;
        }
    }

    public double Price
    {
        get => _price;
        private set
        {
            if (value < 0)
            {
                throw new ArgumentException($"{nameof(Price)} cannot be negative.");
            }

            if (value > MaxPrice)
            {
                throw new ArgumentException($"{nameof(Price)} cannot be greater than {MaxPrice}.");
            }

            _price = value;
        }
    }

    // References 
    public Guid IdUser
    {
        get => _idUser;
        private set
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException($"{nameof(IdUser)} cannot be empty.");
            }

            _idUser = value;
        }
    }

    public User User
    {
        get => _user;
        private set => _user = value;
    }

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

    public AdvertCategory AdvertCategory
    {
        get => _advertCategory;
        private set => _advertCategory = value;
    }

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

    public AdvertLog AdvertLog
    {
        get => _advertLog;
        private set => _advertLog = value;
    }

    public ICollection<Review> Reviews { get; set; }

    // Private constructor used for EF Core
    private Advert()
    {
    }

    /// <summary>
    ///     Factory method used for creating a new Advert.
    /// </summary>
    /// <param name="title"></param>
    /// <param name="description"></param>
    /// <param name="coverImageKey"></param>
    /// <param name="portfolioUrl"></param>
    /// <param name="price"></param>
    /// <param name="idUser"></param>
    /// <param name="idAdvertCategory"></param>
    /// <param name="idAdvertLog"></param>
    /// <returns></returns>
    public static Advert Create(
        string title,
        string description,
        Guid coverImageKey,
        string portfolioUrl,
        double price,
        Guid idUser,
        Guid idAdvertCategory,
        Guid idAdvertLog)
    {
        return new Advert
        {
            IdAdvert = Guid.NewGuid(),
            Title = title,
            Description = description,
            CoverImageKey = coverImageKey,
            PortfolioUrl = portfolioUrl,
            Price = price,
            IdUser = idUser,
            IdAdvertCategory = idAdvertCategory,
            IdAdvertLog = idAdvertLog,
        };
    }

    /// <summary>
    ///     Factory method used for creating a new Advert with a specific IdAdvert.
    ///     Used for seeding purposes.
    /// </summary>
    /// <param name="idAdvert"></param>
    /// <param name="title"></param>
    /// <param name="description"></param>
    /// <param name="coverImageKey"></param>
    /// <param name="portfolioUrl"></param>
    /// <param name="price"></param>
    /// <param name="idUser"></param>
    /// <param name="idAdvertCategory"></param>
    /// <param name="idAdvertLog"></param>
    /// <returns></returns>
    public static Advert CreateWithIdAndStaticData(
        Guid idAdvert,
        string title,
        string description,
        Guid coverImageKey,
        string portfolioUrl,
        double price,
        Guid idUser,
        Guid idAdvertCategory,
        Guid idAdvertLog)
    {
        return new Advert
        {
            IdAdvert = idAdvert,
            Title = title,
            Description = description,
            CoverImageKey = coverImageKey,
            PortfolioUrl = portfolioUrl,
            Price = price,
            IdUser = idUser,
            IdAdvertCategory = idAdvertCategory,
            IdAdvertLog = idAdvertLog,
        };
    }

    public void PartialUpdate(
        string? title,
        string? description,
        string? portfolioUrl,
        double? price)
    {
        if (!string.IsNullOrWhiteSpace(title))
        {
            Title = title;
        }

        if (!string.IsNullOrWhiteSpace(description))
        {
            Description = description;
        }

        if (!string.IsNullOrWhiteSpace(portfolioUrl))
        {
            PortfolioUrl = portfolioUrl;
        }

        if (price is > 0 and <= MaxPrice)
        {
            Price = price.Value;
        }
    }
}