namespace AudioEngineersPlatformBackend.Domain.Entities;

public class SocialMediaName
{
    // Backing fields
    private Guid _idSocialMediaName;
    private string _name = string.Empty;
    private ICollection<SocialMediaLink> _socialMediaLinks;

    // Properties
    public Guid IdSocialMediaName
    {
        get => _idSocialMediaName;
        private set
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException($"{nameof(IdSocialMediaName)} cannot be empty");
            }

            _idSocialMediaName = value;
        }
    }

    public string Name
    {
        get => _name;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"{nameof(Name)} cannot be empty");
            }

            _name = value;
        }
    }

    // References
    public ICollection<SocialMediaLink> SocialMediaLinks
    {
        get => _socialMediaLinks;
        set => _socialMediaLinks = value;
    }

    public static SocialMediaName Create(string name)
    {
        return new SocialMediaName
        {
            IdSocialMediaName = Guid.NewGuid(),
            Name = name
        };
    }

    public static SocialMediaName CreateWithId(Guid idSocialMediaName, string name)
    {
        return new SocialMediaName
        {
            IdSocialMediaName = idSocialMediaName,
            Name = name
        };
    }
}