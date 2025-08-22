using AudioEngineersPlatformBackend.Contracts.Message.GetMessagedUsers;
using AudioEngineersPlatformBackend.Contracts.Message.GetUserData;
using AudioEngineersPlatformBackend.Contracts.Message.GetUserMessages;
using AudioEngineersPlatformBackend.Contracts.Message.SaveAndSendTextMessage;

namespace AudioEngineersPlatformBackend.Application.Abstractions;

public interface IMessageService
{
    Task<TextMessageResponse> SaveTextMessage(TextMessageRequest textMessageRequest,
        CancellationToken cancellationToken);

    Task<List<GetMessageResponse>> GetUserMessages(Guid idUserSender, Guid idUserRecipient,
        CancellationToken cancellationToken);

    Task<List<InteractedUsersResponse>> GetInteractedUsers(Guid idUser, CancellationToken cancellationToken);

    Task<GetUserDataResponse> GetUserData(Guid idUser, CancellationToken cancellationToken);
}