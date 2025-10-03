using MediatR;

namespace AudioEngineersPlatformBackend.Application.CQRS.Chat.Queries.GetInteractedUsers;

public class GetInteractedUsersQuery : IRequest<GetInteractedUsersQueryResult>
{
    public required Guid IdUser { get; set; }
}