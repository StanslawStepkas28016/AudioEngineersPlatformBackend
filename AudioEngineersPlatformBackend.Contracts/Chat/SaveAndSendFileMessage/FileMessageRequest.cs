namespace AudioEngineersPlatformBackend.Contracts.Chat.SaveAndSendFileMessage;

public sealed class FileMessageRequest
{
    public Guid IdUserRecipient { get; set; }
    public string FileName { get; set; } = String.Empty;
    public Guid FileKey { get; set; }
}