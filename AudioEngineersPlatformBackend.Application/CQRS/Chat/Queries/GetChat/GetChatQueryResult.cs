using AudioEngineersPlatformBackend.Application.Dtos;

namespace AudioEngineersPlatformBackend.Application.CQRS.Chat.Queries.GetChat;

public class GetChatQueryResult
{
    public required PagedListDto<ChatMessageDto> PagedChatMessages { get; set; }
}