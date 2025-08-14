namespace AudioEngineersPlatformBackend.Contracts.Advert.AddReview;

public record AddReviewRequest(
    string Content,
    byte SatisfactionLevel
);