namespace AudioEngineersPlatformBackend.Contracts.Chat.SaveAndSendGeneralMessage;

public sealed class MessageResponse
{
    public Guid IdMessage { get; set; }
    public Guid IdUserSender { get; set; }
    public string SenderFirstName { get; set; } = String.Empty;
    public string SenderLastName { get; set; } = String.Empty;
    public string? TextContent { get; set; }
    public string? FileName { get; set; }
    public Guid FileKey { get; set; }
    public string? FileUrl { get; set; }
    public DateTime DateSent { get; set; }
}