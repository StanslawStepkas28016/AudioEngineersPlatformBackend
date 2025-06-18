namespace AudioEngineersPlatformBackend.Contracts.Chat.ReceiveMessage;

public sealed class ReceiveIsOnlineMessage
{
    public Guid IdUser { get; set; }
    public bool IsOnline { get; set; }
}