using AudioEngineersPlatformBackend.Application.Dtos;

namespace AudioEngineersPlatformBackend.Application.CQRS.Advert.Queries.GetAdvertReviews;

public class GetAdvertReviewsQueryResult
{
    public required PagedListDto<ReviewDto> PagedAdvertReviews { get; set; }
}