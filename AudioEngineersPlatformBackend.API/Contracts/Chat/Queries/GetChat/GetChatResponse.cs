using AudioEngineersPlatformBackend.Application.Dtos;

namespace API.Contracts.Chat.Queries.GetChat;

public class GetChatResponse
{
    public required PagedListDto<ChatMessageDto> PagedChatMessages { get; set; }
}