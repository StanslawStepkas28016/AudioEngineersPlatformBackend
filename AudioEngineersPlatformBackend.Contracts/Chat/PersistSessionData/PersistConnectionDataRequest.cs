namespace AudioEngineersPlatformBackend.Contracts.Chat.PersistSessionData;

public sealed class PersistConnectionDataRequest
{
    public string ConnectionId { get; set; }
    public bool IsConnecting { get; set; }
}