using MediatR;

namespace AudioEngineersPlatformBackend.Application.CQRS.Advert.Commands.AddReview;

public class AddReviewCommand : IRequest<AddReviewCommandResult>
{
    public required Guid IdUserReviewer { get; set; }
    public required Guid IdAdvert { get; set; }
    public required string Content { get; set; } 
    public required byte SatisfactionLevel { get; set; }
}