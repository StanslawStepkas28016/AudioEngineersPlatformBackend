using AudioEngineersPlatformBackend.Application.CQRS.Advert.Queries.GetAllAdvertsSumarries;
using AudioEngineersPlatformBackend.Application.Dtos;
using AutoMapper;

namespace API.Contracts.Advert.Queries.GetAllAdvertsSummaries;

public class GetAllAdvertsSummariesProfile : Profile
{
    public GetAllAdvertsSummariesProfile()
    {
        CreateMap<GetAllAdvertsSummariesRequest, GetAllAdvertsSummariesQuery>();

        CreateMap<PagedListDto<AdvertSummaryDto>, GetAllAdvertsSummariesQueryResult>()
            .ForMember
            (
                dest => dest.PagedAdvertSummaries,
                exp => exp.MapFrom
                ((
                        src,
                        dest
                    ) => dest.PagedAdvertSummaries = src
                )
            );

        CreateMap<GetAllAdvertsSummariesQueryResult, GetAllAdvertsSummariesResponse>();
    }
}