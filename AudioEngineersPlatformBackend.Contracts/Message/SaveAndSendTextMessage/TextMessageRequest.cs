namespace AudioEngineersPlatformBackend.Contracts.Message.SaveAndSendTextMessage;

public record TextMessageRequest
{
    public Guid IdUserSender { get; set; }
    public Guid IdUserRecipient { get; set; }
    public string TextContent { get; set; }
}