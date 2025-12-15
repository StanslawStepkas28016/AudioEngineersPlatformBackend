namespace API.Contracts.Chat.Commands.PersistConnectionData;

public class PersistConnectionDataRequest
{
    public required Guid IdUser { get; set; }
    public required string ConnectionId { get; set; }
    public required bool IsConnecting { get; set; }
}