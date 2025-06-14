namespace AudioEngineersPlatformBackend.Domain.Entities;

public class Advert
{
    public Guid IdAdvert { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public string CoverImageUrl { get; private set; }
    public string PortfolioUrl { get; private set; }
    public double Price { get; private set; }

    public Guid IdAdvertCategory { get; private set; }
    public AdvertCategory AdvertCategory { get; private set; }
    public Guid IdAdvertLog { get; private set; }
    public AdvertLog AdvertLog { get; private set; }

    // Constants
    private const double MaxPrice = 1500.0;
    private const int MaxTitleLength = 100;
    private const int MaxDescriptionLength = 500;

    private Advert()
    {
    }

    public Advert(string title, string description, string coverImageUrl, string portfolioUrl, double price,
        Guid idAdvertCategory, Guid idAdvertLog)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title cannot be null or whitespace.", nameof(title));
        }

        if (title.Length > MaxTitleLength)
        {
            throw new ArgumentException($"Title cannot exceed {MaxTitleLength} characters.", nameof(title));
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("Description cannot be null or whitespace.", nameof(description));
        }

        if (description.Length > MaxDescriptionLength)
        {
            throw new ArgumentException($"Description cannot exceed {MaxDescriptionLength} characters.",
                nameof(description));
        }

        if (string.IsNullOrWhiteSpace(coverImageUrl))
        {
            throw new ArgumentException("Cover image URL cannot be null or whitespace.", nameof(coverImageUrl));
        }

        if (string.IsNullOrWhiteSpace(portfolioUrl))
        {
            throw new ArgumentException("Portfolio URL cannot be null or whitespace.", nameof(portfolioUrl));
        }

        if (price < 0)
        {
            throw new ArgumentException("Price cannot be negative.", nameof(price));
        }

        if (price > MaxPrice)
        {
            throw new ArgumentException($"Price cannot be greater than {MaxPrice}.", nameof(price));
        }

        if (idAdvertCategory == Guid.Empty)
        {
            throw new ArgumentException("Category ID cannot be empty.", nameof(idAdvertCategory));
        }

        if (idAdvertLog == Guid.Empty)
        {
            throw new ArgumentException("AdvertLog ID cannot be empty.", nameof(idAdvertLog));
        }

        IdAdvert = Guid.NewGuid();
        Title = title;
        Description = description;
        CoverImageUrl = coverImageUrl;
        PortfolioUrl = portfolioUrl;
        Price = price;
        IdAdvertCategory = idAdvertCategory;
        IdAdvertLog = idAdvertLog;
    }
}