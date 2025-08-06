namespace AudioEngineersPlatformBackend.Domain.Entities;

public class ReviewLog
{
    // Backing fields
    private Guid _idReviewLog;
    private DateTime _dateCreated;
    private DateTime? _dateDeleted;
    private bool _isDeleted;
    private ICollection<Review> _reviews;

    // Properties
    public Guid IdReviewLog
    {
        get => _idReviewLog;
        private set
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException($"{nameof(IdReviewLog)} cannot be empty.");
            }

            _idReviewLog = value;
        }
    }

    public DateTime DateCreated
    {
        get => _dateCreated;
        private set => _dateCreated = value;
    }

    public DateTime? DateDeleted
    {
        get => _dateDeleted;
        private set => _dateDeleted = value;
    }

    public bool IsDeleted
    {
        get => _isDeleted;
        private set => _isDeleted = value;
    }

    // References
    public ICollection<Review> Reviews
    {
        get => _reviews;
        private set => _reviews = value;
    }

    /// <summary>
    ///     Factory method used to create a new ReviewLog instance.
    /// </summary>
    /// <returns></returns>
    public static ReviewLog Create()
    {
        return new ReviewLog
        {
            IdReviewLog = Guid.NewGuid(),
            DateCreated = DateTime.UtcNow,
            DateDeleted = null,
            IsDeleted = false,
        };
    }

    /// <summary>
    ///     Factory method used to create a new ReviewLog instance with a provided idReviewLog.
    /// </summary>
    /// <param name="idReviewLog"></param>
    /// <returns></returns>
    public static ReviewLog CreateWithIdAndStaticData(Guid idReviewLog, DateTime dateCreated)
    {
        return new ReviewLog
        {
            IdReviewLog = idReviewLog,
            DateCreated = dateCreated,
            DateDeleted = null,
            IsDeleted = false,
        };
    }

    /// <summary>
    ///     Method used for soft deleting a ReviewLog.
    /// </summary>
    public void SoftDelete()
    {
        DateDeleted = DateTime.UtcNow;
        IsDeleted = true;
    }
}