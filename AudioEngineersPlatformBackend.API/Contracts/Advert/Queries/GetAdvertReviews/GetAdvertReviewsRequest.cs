namespace API.Contracts.Advert.Queries.GetAdvertReviews;

public class GetAdvertReviewsRequest
{
    public required int Page { get; set; }
    public required int PageSize { get; set; }
}