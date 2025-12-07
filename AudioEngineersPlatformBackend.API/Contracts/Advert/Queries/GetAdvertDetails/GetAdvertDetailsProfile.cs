using AudioEngineersPlatformBackend.Application.CQRS.Advert.Queries.GetAdvertDetails;
using AudioEngineersPlatformBackend.Application.Dtos;
using AutoMapper;

namespace API.Contracts.Advert.Queries.GetAdvertDetails;

public class GetAdvertDetailsProfile : Profile
{
    public GetAdvertDetailsProfile()
    {
        CreateMap<Guid, GetAdvertDetailsRequest>()
            .ForMember
            (
                dest => dest.IdAdvert,
                opt => opt.MapFrom
                ((
                        src,
                        dest
                    ) => dest.IdAdvert = src
                )
            );

        CreateMap<GetAdvertDetailsRequest, GetAdvertDetailsQuery>();

        CreateMap<AdvertDetailsDto, GetAdvertDetailsQueryResult>();

        CreateMap<GetAdvertDetailsQueryResult, GetAdvertDetailsResponse>();
    }
}