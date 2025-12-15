using MediatR;

namespace AudioEngineersPlatformBackend.Application.CQRS.Advert.Queries.GetAdvertReviews;

public class GetAdvertReviewsQuery : IRequest<GetAdvertReviewsQueryResult>
{
    public required Guid IdAdvert { get; set; }
    public required int Page { get; set; }
    public required int PageSize { get; set; }
}