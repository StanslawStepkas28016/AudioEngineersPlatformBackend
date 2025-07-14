namespace AudioEngineersPlatformBackend.Contracts.Advert.AddReview;

public record AddReviewRequest(
    Guid IdAdvert,
    string Content,
    byte SatisfactionLevel
);