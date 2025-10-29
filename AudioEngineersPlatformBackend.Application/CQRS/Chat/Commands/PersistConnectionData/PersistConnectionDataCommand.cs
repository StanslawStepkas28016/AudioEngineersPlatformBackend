using MediatR;

namespace AudioEngineersPlatformBackend.Application.CQRS.Chat.Commands.PersistConnectionData;

public class PersistConnectionDataCommand : IRequest<PersistConnectionDataCommandResult>
{
    public required Guid IdUser { get; set; }
    public required string ConnectionId { get; set; }
    public required bool IsConnecting { get; set; }
}