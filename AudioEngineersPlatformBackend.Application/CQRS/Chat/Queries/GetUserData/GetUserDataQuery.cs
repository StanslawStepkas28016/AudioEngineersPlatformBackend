using MediatR;

namespace AudioEngineersPlatformBackend.Application.CQRS.Chat.Queries.GetUserData;

public class GetUserDataQuery : IRequest<GetUserDataQueryResult>
{
    public required Guid IdUser { get; set; }
}