namespace API.Contracts.Chat.Commands.SendFileMessage;

public class SendFileMessageRequest
{
    public required Guid IdUserSender { get; set; }
    public required Guid IdUserRecipient { get; set; }
    public required Guid FileKey { get; set; }
    public required string FileName { get; set; } 
}