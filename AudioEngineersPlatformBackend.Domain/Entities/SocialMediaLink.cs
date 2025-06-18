namespace AudioEngineersPlatformBackend.Domain.Entities;

public class SocialMediaLink
{
    // Backing fields
    private Guid _idSocialMediaLink;
    private string _url;
    private Guid _idUser;
    private User _user;
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
                throw new ArgumentException($"{nameof(IdSocialMediaLink)} cannot be empty.");
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
                throw new ArgumentException($"{nameof(Url)} cannot be empty.");
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
                throw new ArgumentException($"{nameof(Url)} is invalid.");
            }

            if (!AllowedUrls.Contains(uri.Authority))
            {
                throw new ArgumentException(
                    $"You can only use {nameof(Url).ToLower()}s of {string.Join(' ', AllowedUrls)}.");
            }

            _url = value;
        }
    }

    // References
    public Guid IdUser
    {
        get => _idUser;
        private set => _idUser = value;
    }

    public User User
    {
        get => _user;
        private set => _user = value;
    }

    public Guid IdSocialMediaName
    {
        get => _idSocialMediaName;
        private set
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException($"{nameof(IdSocialMediaName)} cannot be empty.");
            }

            _idSocialMediaName = value;
        }
    }

    public SocialMediaName SocialMediaName
    {
        get => _socialMediaName;
        private set => _socialMediaName = value;
    }
    
    // Methods

    // Private constructor for EF Core
    private SocialMediaLink()
    {
    }

    public static SocialMediaLink Create(Guid idUser, string? url, Guid idSocialMediaName)
    {
        return new SocialMediaLink
        {
            IdSocialMediaLink = Guid.NewGuid(),
            IdUser = idUser,
            Url = url,
            IdSocialMediaName = idSocialMediaName
        };
    }

    public static SocialMediaLink CreateWithId(Guid idSocialMediaLink, Guid idUser, string? url, Guid idSocialMediaName)
    {
        return new SocialMediaLink
        {
            IdSocialMediaLink = idSocialMediaLink,
            IdUser = idUser,
            Url = url,
            IdSocialMediaName = idSocialMediaName
        };
    }
}