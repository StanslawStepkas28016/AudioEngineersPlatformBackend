namespace AudioEngineersPlatformBackend.Domain.Entities;

public class Review
{
    // Backing fields
    private Guid _idReview;
    private string _content = string.Empty;
    private short _satisfactionLevel;
    private Guid _idAdvert;
    private Guid _idReviewLog;
    private Advert? _advert;
    private ReviewLog? _reviewLog;

    // Constants
    private const short MinSatisfactionLevel = 1;
    private const short MaxSatisfactionLevel = 5;
    private const int MinContentLength = 35;
    private const int MaxContentLength = 1500;

    // Properties
    public Guid IdReview
    {
        get => _idReview;
        private set
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException("IdReview cannot be empty", nameof(value));
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
                throw new ArgumentException("Content cannot be empty", nameof(value));
            }

            if (value.Length > MaxContentLength || value.Length < MinContentLength)
            {
                throw new ArgumentException(
                    $"Content must be between {MinContentLength} and {MaxContentLength} characters long.",
                    nameof(value));
            }

            _content = value;
        }
    }

    public short SatisfactionLevel
    {
        get => _satisfactionLevel;
        private set
        {
            if (_satisfactionLevel < MinSatisfactionLevel || _satisfactionLevel > MaxSatisfactionLevel)
            {
                throw new ArgumentException(
                    $"SatisfactionLevel must be between {MinSatisfactionLevel} and {MaxSatisfactionLevel}.",
                    nameof(value));
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
                throw new ArgumentException("IdAdvert cannot be empty", nameof(value));
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
                throw new ArgumentException("IdReviewLog cannot be empty", nameof(value));
            }

            _idReviewLog = value;
        }
    }

    public ReviewLog ReviewLog
    {
        get => _reviewLog;
        private set => _reviewLog = value;
    }

    // Private constructor for EF Core
    private Review()
    {
    }

    /// <summary>
    ///     Factory method used to create a new Review instance.
    /// </summary>
    /// <param name="idAdvert"></param>
    /// <param name="content"></param>
    /// <param name="satisfactionLevel"></param>
    /// <param name="idReviewLog"></param>
    /// <returns></returns>
    public Review CreateReview(Guid idAdvert, string content, short satisfactionLevel, Guid idReviewLog)
    {
        return new Review
        {
            IdReview = Guid.NewGuid(),
            IdAdvert = idAdvert,
            Content = content,
            SatisfactionLevel = satisfactionLevel,
            IdReviewLog = idReviewLog
        };
    }

    /// <summary>
    ///     Method used to create a new Review instance with a specific IdReview.
    /// </summary>
    /// <param name="idReview"></param>
    /// <param name="idAdvert"></param>
    /// <param name="content"></param>
    /// <param name="satisfactionLevel"></param>
    /// <param name="idReviewLog"></param>
    /// <returns></returns>
    public Review CreateReviewWithId(Guid idReview, Guid idAdvert, string content, short satisfactionLevel, Guid idReviewLog)
    {
        return new Review
        {
            IdReview = idReview,
            IdAdvert = idAdvert,
            Content = content,
            SatisfactionLevel = satisfactionLevel,
            IdReviewLog = idReviewLog
        };
    }
}