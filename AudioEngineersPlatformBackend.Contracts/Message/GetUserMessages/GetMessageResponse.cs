namespace AudioEngineersPlatformBackend.Contracts.Message.GetUserMessages;

public sealed class GetMessageResponse
{
    public Guid IdMessage { get; set; }
    public Guid IdUserSender { get; set; }
    public string? TextContent { get; set; }
    public Guid FileKey { get; set; }
    public string? FileUrl { get; set; }
    public DateTime DateSent { get; set; }
};