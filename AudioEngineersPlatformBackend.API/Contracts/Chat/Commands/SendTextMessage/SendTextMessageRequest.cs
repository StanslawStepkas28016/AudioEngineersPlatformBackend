namespace API.Contracts.Chat.Commands.SendTextMessage;

public class SendTextMessageRequest
{
    public required Guid IdUserSender { get; set; }
    public required Guid IdUserRecipient { get; set; }
    public required  string TextContent { get; set; } 
}