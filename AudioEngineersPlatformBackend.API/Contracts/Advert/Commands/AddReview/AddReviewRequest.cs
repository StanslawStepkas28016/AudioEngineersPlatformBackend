namespace API.Contracts.Advert.Commands.AddReview;

public class AddReviewRequest
{
    public required Guid IdUserReviewer { get; set; }
    public required string Content { get; set; } 
    public required byte SatisfactionLevel { get; set; }
}