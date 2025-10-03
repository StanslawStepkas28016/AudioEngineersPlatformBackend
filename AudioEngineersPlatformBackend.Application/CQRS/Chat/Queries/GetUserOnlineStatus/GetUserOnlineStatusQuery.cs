using MediatR;

namespace AudioEngineersPlatformBackend.Application.CQRS.Chat.Queries.GetUserOnlineStatus;

public class GetUserOnlineStatusQuery : IRequest<GetUserOnlineStatusQueryResult>
{
    public required Guid IdUser { get; set; }
}