namespace API.Contracts.Chat.Queries.GetChat;

public class GetChatRequest
{
    public required int Page { get; set; }
    public required int PageSize { get; set; }
}