using AudioEngineersPlatformBackend.Application.CQRS.Advert.Queries.GetIdAdvertByIdUser;
using AutoMapper;

namespace API.Contracts.Advert.Queries.GetIdAdvertByIdUser;

public class GetIdAdvertByIdUserProfile : Profile
{
    public GetIdAdvertByIdUserProfile()
    {
        CreateMap<GetIdAdvertByIdUserRequest, GetIdAdvertByIdUserQuery>();

        CreateMap<Guid, GetIdAdvertByIdUserQueryResult>()
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

        CreateMap<GetIdAdvertByIdUserQueryResult, GetIdAdvertByIdUserResponse>();
    }
}