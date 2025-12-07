namespace API.Contracts.Chat.Commands.SendTextMessage;

public class SendTextMessageResponse
{
    public required Guid IdMessage { get; set; }
    public required Guid IdUserSender { get; set; }
    public required string SenderFirstName { get; set; } 
    public required string SenderLastName { get; set; } 
    public required string TextContent { get; set; } 
    public required string FileName { get; set; } 
    public required Guid FileKey { get; set; }
    public required string FileUrl { get; set; } 
    public required DateTime DateSent { get; set; }
}