using AudioEngineersPlatformBackend.Application.CQRS.Chat.Queries.GetUserData;
using AudioEngineersPlatformBackend.Application.Dtos;
using AutoMapper;

namespace API.Contracts.Chat.Queries.GetUserData;

public class GetUserDataProfile : Profile
{
    public GetUserDataProfile()
    {
        CreateMap<Guid, GetUserDataQuery>()
            .ForMember
            (
                dest => dest.IdUser,
                opt => opt.MapFrom
                ((
                        src,
                        dest
                    ) => dest.IdUser = src
                )
            );

        CreateMap<UserDataDto, GetUserDataQueryResult>();

        CreateMap<GetUserDataQueryResult, GetUserDataResponse>();
    }
}