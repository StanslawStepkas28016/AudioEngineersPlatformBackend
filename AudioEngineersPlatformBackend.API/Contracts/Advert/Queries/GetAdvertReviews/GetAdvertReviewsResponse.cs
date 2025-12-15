using AudioEngineersPlatformBackend.Application.Dtos;

namespace API.Contracts.Advert.Queries.GetAdvertReviews;

public class GetAdvertReviewsResponse
{
    public required PagedListDto<ReviewDto> PagedAdvertReviews { get; set; }
}