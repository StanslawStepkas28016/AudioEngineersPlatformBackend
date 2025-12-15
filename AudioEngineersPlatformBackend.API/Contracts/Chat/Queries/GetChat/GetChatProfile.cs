using AudioEngineersPlatformBackend.Application.CQRS.Chat.Queries.GetChat;
using AudioEngineersPlatformBackend.Application.Dtos;
using AutoMapper;

namespace API.Contracts.Chat.Queries.GetChat;

public class GetChatProfile : Profile
{
    public GetChatProfile()
    {
        CreateMap<GetChatRequest, GetChatQuery>()
            .ForMember(dest => dest.IdUserSender, opt => opt.Ignore())
            .ForMember(dest => dest.IdUserRecipient, opt => opt.Ignore());

        CreateMap<PagedListDto<ChatMessageDto>, GetChatQueryResult>()
            .ForMember
            (
                exp => exp.PagedChatMessages,
                opt => opt.MapFrom
                ((
                        src,
                        dest
                    ) => dest.PagedChatMessages = src
                )
            );

        CreateMap<GetChatQueryResult, GetChatResponse>();
    }
}