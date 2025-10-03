using MediatR;

namespace AudioEngineersPlatformBackend.Application.CQRS.Chat.Commands.SendTextMessage;

public class SendTextMessageCommand : IRequest<SendTextMessageCommandResult>
{
    public required Guid IdUserSender { get; set; }
    public required Guid IdUserRecipient { get; set; }
    public required string TextContent { get; set; } 
}