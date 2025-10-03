using MediatR;

namespace AudioEngineersPlatformBackend.Application.CQRS.Chat.Commands.SendFileMessage;

public class SendFileMessageCommand : IRequest<SendFileMessageCommandResult>
{
    public required Guid IdUserSender { get; set; }
    public required Guid IdUserRecipient { get; set; }
    public required Guid FileKey { get; set; }
    public required string FileName { get; set; } 
}