namespace AudioEngineersPlatformBackend.Contracts.Chat.GetChat;

public sealed class GetChatResponse
{
    public Guid IdMessage { get; set; }
    public Guid IdUserSender { get; set; }
    public bool IsRead { get; set; }
    public string? TextContent { get; set; }
    public Guid FileKey { get; set; }
    public string? FileName { get; set; }
    public string? FileUrl { get; set; }
    public DateTime DateSent { get; set; }
};