using AudioEngineersPlatformBackend.Application.Dtos;

namespace API.Contracts.Advert.Queries.GetAllAdvertsSummaries;

public class GetAllAdvertsSummariesResponse
{
    public required PagedListDto<AdvertSummaryDto> PagedAdvertSummaries { get; set; }
}