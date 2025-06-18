namespace AudioEngineersPlatformBackend.Contracts.Chat.SaveAndSendTextMessage;

public sealed class TextMessageRequest
{
    public Guid IdUserRecipient { get; set; }
    public string TextContent { get; set; }
}