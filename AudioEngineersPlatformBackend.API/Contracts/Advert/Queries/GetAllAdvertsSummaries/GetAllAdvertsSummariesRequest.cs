namespace API.Contracts.Advert.Queries.GetAllAdvertsSummaries;

public class GetAllAdvertsSummariesRequest
{
    public string? CategoryFilterTerm { get; set; }
    public string? SortOrder { get; set; }
    public string? SearchTerm { get; set; }
    public required int Page { get; set; }
    public required int PageSize { get; set; }
}