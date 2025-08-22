namespace AudioEngineersPlatformBackend.Contracts.Message.SaveAndSendTextMessage;

public sealed class TextMessageRequest
{
    public Guid IdUserSender { get; set; }
    public Guid IdUserRecipient { get; set; }
    public string TextContent { get; set; }
}