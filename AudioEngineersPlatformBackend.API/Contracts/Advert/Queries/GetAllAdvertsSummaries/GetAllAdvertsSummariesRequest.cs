namespace API.Contracts.Advert.Queries.GetAllAdvertsSummaries;

public class GetAllAdvertsSummariesRequest
{
    public string? SortOrder { get; set; }
    public required int Page { get; set; }
    public required int PageSize { get; set; }
    public string? SearchTerm { get; set; }
}