using AudioEngineersPlatformBackend.Application.CQRS.Chat.Queries.GetUserOnlineStatus;
using AutoMapper;

namespace API.Contracts.Chat.Queries.GetUserOnlineStatus;

public class GetUserOnlineStatusProfile : Profile
{
    public GetUserOnlineStatusProfile()
    {
        CreateMap<Guid, GetUserOnlineStatusQuery>()
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

        CreateMap<bool, GetUserOnlineStatusQueryResult>()
            .ForMember
            (
                dest => dest.IsOnline,
                opt => opt.MapFrom
                ((
                        src,
                        dest
                    ) => dest.IsOnline = src
                )
            );

        CreateMap<GetUserOnlineStatusQueryResult, GetUserOnlineStatusResponse>();
    }
}