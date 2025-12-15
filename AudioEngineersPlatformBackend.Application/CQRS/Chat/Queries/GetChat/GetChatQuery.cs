using MediatR;

namespace AudioEngineersPlatformBackend.Application.CQRS.Chat.Queries.GetChat;

public class GetChatQuery : IRequest<GetChatQueryResult>
{
    public required Guid IdUserSender { get; set; }
    public required Guid IdUserRecipient { get; set; }
    public required int Page { get; set; }
    public required int PageSize { get; set; }
}