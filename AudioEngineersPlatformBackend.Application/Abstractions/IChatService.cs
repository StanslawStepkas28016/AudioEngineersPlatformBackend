using AudioEngineersPlatformBackend.Contracts.Chat.GetChat;
using AudioEngineersPlatformBackend.Contracts.Chat.GetMessagedUsers;
using AudioEngineersPlatformBackend.Contracts.Chat.GetPresignedUrlForUpload;
using AudioEngineersPlatformBackend.Contracts.Chat.GetUserData;
using AudioEngineersPlatformBackend.Contracts.Chat.PersistSessionData;
using AudioEngineersPlatformBackend.Contracts.Chat.SaveAndSendFileMessage;
using AudioEngineersPlatformBackend.Contracts.Chat.SaveAndSendGeneralMessage;
using AudioEngineersPlatformBackend.Contracts.Chat.SaveAndSendTextMessage;

namespace AudioEngineersPlatformBackend.Application.Abstractions;

public interface IChatService
{
    Task<MessageResponse> SaveTextMessage(Guid idUserSender, TextMessageRequest textMessageRequest,
        CancellationToken cancellationToken);

    Task<List<GetChatResponse>> GetChat(Guid idUserSender, Guid idUserRecipient,
        CancellationToken cancellationToken);

    Task<List<InteractedUsersResponse>> GetInteractedUsers(Guid idUser, CancellationToken cancellationToken);

    Task<GetUserDataResponse> GetUserData(Guid idUser, CancellationToken cancellationToken);

    Task PersistConnectionData(Guid idUser, PersistConnectionDataRequest persistConnectionDataRequest,
        CancellationToken cancellationToken);

    Task<bool> GetUserOnlineStatus(Guid idUser, CancellationToken cancellationToken);

    Task<MessageResponse> SaveFileMessage(Guid idUserSender, FileMessageRequest fileMessageRequest,
        CancellationToken cancellationToken);

    Task<GetPresignedUrlForUploadResponse> GetPresignedUrlForUpload(string folder, string fileName, 
        CancellationToken cancellationToken);
}