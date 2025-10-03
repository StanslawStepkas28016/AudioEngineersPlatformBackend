namespace AudioEngineersPlatformBackend.Domain.Entities;

public class Review
{
    // Backing fields
    private Guid _idReview;
    private string _content;
    private byte _satisfactionLevel;
    private Guid _idAdvert;
    private Guid _idReviewLog;
    private Advert? _advert;
    private ReviewLog? _reviewLog;
    private Guid _idUser;
    private User _user;

    // Constants
    public const byte MinSatisfactionLevel = 1;
    public const byte MaxSatisfactionLevel = 5;
    public const int MinContentLength = 35;
    public const int MaxContentLength = 1500;

    // Properties
    public Guid IdReview
    {
        get => _idReview;
        private set
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException($"{nameof(IdReview)} cannot be empty.");
            }

            _idReview = value;
        }
    }

    public string Content
    {
        get => _content;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"{nameof(Content)} cannot be empty.");
            }

            if (value.Length > MaxContentLength || value.Length < MinContentLength)
            {
                throw new ArgumentException(
                    $"{nameof(Content)} must be between {MinContentLength} and {MaxContentLength} characters long.");
            }

            _content = value;
        }
    }

    public byte SatisfactionLevel
    {
        get => _satisfactionLevel;
        private set
        {
            if (value < MinSatisfactionLevel || value > MaxSatisfactionLevel)
            {
                throw new ArgumentException(
                    $"{nameof(SatisfactionLevel)} must be between {MinSatisfactionLevel} and {MaxSatisfactionLevel}.");
            }

            _satisfactionLevel = value;
        }
    }

    // References 
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

    public Advert Advert
    {
        get => _advert;
        private set => _advert = value;
    }

    public Guid IdReviewLog
    {
        get => _idReviewLog;
        set
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException($"{nameof(IdReviewLog)} cannot be empty.");
            }

            _idReviewLog = value;
        }
    }

    public ReviewLog ReviewLog
    {
        get => _reviewLog;
        private set => _reviewLog = value;
    }

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
        set => _user = value;
    }
    
    // Private constructor for EF Core
    private Review()
    {
    }

    /// <summary>
    ///     Factory method used to create a new Review instance.
    /// </summary>
    /// <param name="idAdvert"></param>
    /// <param name="idReviewLog"></param>
    /// <param name="idUser"></param>
    /// <param name="content"></param>
    /// <param name="satisfactionLevel"></param>
    /// <returns></returns>
    public static Review Create(Guid idAdvert, Guid idReviewLog, Guid idUser, string content, byte satisfactionLevel)
    {
        return new Review
        {
            IdReview = Guid.NewGuid(),
            IdAdvert = idAdvert,
            IdReviewLog = idReviewLog,
            IdUser = idUser,
            Content = content,
            SatisfactionLevel = satisfactionLevel
        };
    }

    /// <summary>
    ///     Method used to create a new Review instance with a specific IdReview.
    /// </summary>
    /// <param name="idReview"></param>
    /// <param name="idAdvert"></param>
    /// <param name="idReviewLog"></param>
    /// <param name="idUser"></param>
    /// <param name="content"></param>
    /// <param name="satisfactionLevel"></param>
    /// <returns></returns>
    /// 
    public static Review CreateWithIdAndStaticData(Guid idReview, Guid idAdvert, Guid idReviewLog, Guid idUser, string content,
        byte satisfactionLevel)
    {
        return new Review
        {
            IdReview = idReview,
            IdAdvert = idAdvert,
            IdReviewLog = idReviewLog,
            IdUser = idUser,
            Content = content,
            SatisfactionLevel = satisfactionLevel
        };
    }
}