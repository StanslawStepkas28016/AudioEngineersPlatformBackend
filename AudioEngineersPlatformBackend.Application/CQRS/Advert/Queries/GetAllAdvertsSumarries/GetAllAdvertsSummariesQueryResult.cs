using AudioEngineersPlatformBackend.Application.Dtos;

namespace AudioEngineersPlatformBackend.Application.CQRS.Advert.Queries.GetAllAdvertsSumarries;

public class GetAllAdvertsSummariesQueryResult
{
    public required PagedListDto<AdvertSummaryDto> PagedAdvertSummaries { get; set; }
}