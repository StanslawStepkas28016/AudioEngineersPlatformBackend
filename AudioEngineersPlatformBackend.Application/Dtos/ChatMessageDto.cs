namespace AudioEngineersPlatformBackend.Application.Dtos;

public class ChatMessageDto
{
    public required Guid IdMessage { get; set; }
    public required Guid IdUserSender { get; set; }
    public required bool IsRead { get; set; }
    public required string TextContent { get; set; }
    public required Guid FileKey { get; set; }
    public required string FileName { get; set; }
    public required string FileUrl { get; set; }
    public required DateTime DateSent { get; set; }
}