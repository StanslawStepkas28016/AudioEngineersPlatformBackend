using MediatR;

namespace AudioEngineersPlatformBackend.Application.CQRS.Advert.Queries.GetAllAdvertsSumarries;

public class GetAllAdvertsSummariesQuery : IRequest<GetAllAdvertsSummariesQueryResult>
{
    public string? CategoryFilterTerm { get; set; }
    public string? SortOrder { get; set; }
    public string? SearchTerm { get; set; }
    public required int Page { get; set; }
    public required int PageSize { get; set; }
}