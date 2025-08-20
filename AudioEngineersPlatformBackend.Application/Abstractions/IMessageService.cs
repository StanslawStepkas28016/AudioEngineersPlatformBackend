using AudioEngineersPlatformBackend.Contracts.Message.GetUserMessages;
using AudioEngineersPlatformBackend.Contracts.Message.SaveAndSendTextMessage;

namespace AudioEngineersPlatformBackend.Application.Abstractions;

public interface IMessageService
{
    Task SaveTextMessage(TextMessageRequest textMessageRequest,
        CancellationToken cancellationToken);

    Task<List<GetMessageResponse>> GetUserMessages(Guid idUserSender, Guid idUserRecipient,
        CancellationToken cancellationToken);
}