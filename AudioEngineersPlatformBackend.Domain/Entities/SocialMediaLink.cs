namespace AudioEngineersPlatformBackend.Domain.Entities;

public class SocialMediaLink
{
    // Backing fields
    private Guid _idSocialMediaLink;
    private string _url;
    private Guid _idAdvert;
    private Advert _advert;
    private Guid _idSocialMediaName;
    private SocialMediaName _socialMediaName;

    // Constants
    private static readonly string[] AllowedUrls =
        ["www.instagram.com", "www.linkedin.com", "www.facebook.com"];

    // Properties
    public Guid IdSocialMediaLink
    {
        get => _idSocialMediaLink;
        private set
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException("IdSocialMediaLink cannot be empty", nameof(IdSocialMediaLink));
            }

            _idSocialMediaLink = value;
        }
    }

    public string? Url
    {
        get => _url;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Url cannot be empty", nameof(Url));
            }

            // Validating the URL for only the allowed ones
            Uri uri;
            try
            {
                uri = new Uri(value);
            }
            // Any errors from the input Url is going to be caught here
            catch (Exception e)
            {
                throw new ArgumentException("Url is invalid", nameof(Url));
            }

            if (!AllowedUrls.Contains(uri.Authority))
            {
                throw new ArgumentException($"You can only use urls of {
                    string.Join(' ', AllowedUrls)
                }", nameof(Url));
            }

            _url = value;
        }
    }

    // References
    public Guid IdAdvert
    {
        get => _idAdvert;
        private set => _idAdvert = value;
    }

    public Advert Advert
    {
        get => _advert;
        private set => _advert = value;
    }

    public Guid IdSocialMediaName
    {
        get => _idSocialMediaName;
        private set
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException("IdSocialMediaName cannot be empty.", nameof(value));
            }

            _idSocialMediaName = value;
        }
    }

    public SocialMediaName SocialMediaName
    {
        get => _socialMediaName;
        private set => _socialMediaName = value;
    }

    // Private constructor for EF Core
    private SocialMediaLink()
    {
    }

    public static SocialMediaLink Create(Guid idAdvert, string? url, Guid idSocialMediaName)
    {
        return new SocialMediaLink
        {
            IdSocialMediaLink = Guid.NewGuid(),
            IdAdvert = idAdvert,
            Url = url,
            IdSocialMediaName = idSocialMediaName
        };
    }

    public static SocialMediaLink CreateWithId(Guid idSocialMediaLink, Guid idAdvert, string? url, Guid idSocialMediaName)
    {
        return new SocialMediaLink
        {
            IdSocialMediaLink = idSocialMediaLink,
            IdAdvert = idAdvert,
            Url = url,
            IdSocialMediaName = idSocialMediaName
        };
    }
}