using AudioEngineersPlatformBackend.Application.CQRS.Chat.Queries.GetInteractedUsers;
using AudioEngineersPlatformBackend.Application.Dtos;
using AutoMapper;

namespace API.Contracts.Chat.Queries.GetInteractedUsers;

public class GetInteractedUsersProfile : Profile
{
    public GetInteractedUsersProfile()
    {
        CreateMap<Guid, GetInteractedUsersQuery>()
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

        CreateMap<List<InteractedUserDto>, GetInteractedUsersQueryResult>()
            .ForMember
            (
                dest => dest.InteractedUsersList,
                opt => opt.MapFrom
                ((
                        src,
                        dest
                    ) => dest.InteractedUsersList = src
                )
            );

        CreateMap<GetInteractedUsersQueryResult, GetInteractedUsersResponse>();
    }
}