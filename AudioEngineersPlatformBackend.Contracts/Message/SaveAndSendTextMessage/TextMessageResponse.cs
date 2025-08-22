namespace AudioEngineersPlatformBackend.Contracts.Message.SaveAndSendTextMessage;

public sealed class TextMessageResponse
{
    public Guid IdMessage { get; set; }
    public Guid IdUserSender { get; set; }
    public string? TextContent { get; set; }
    public Guid FileKey { get; set; }
    public string? FileUrl { get; set; }
    public DateTime DateSent { get; set; }
}