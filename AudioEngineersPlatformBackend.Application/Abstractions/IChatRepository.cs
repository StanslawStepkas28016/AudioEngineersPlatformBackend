using AudioEngineersPlatformBackend.Contracts.Chat.GetChat;
using AudioEngineersPlatformBackend.Contracts.Chat.GetMessagedUsers;
using AudioEngineersPlatformBackend.Contracts.Chat.GetUserData;
using AudioEngineersPlatformBackend.Domain.Entities;

namespace AudioEngineersPlatformBackend.Application.Abstractions;

public interface IChatRepository
{
    Task SaveMessageAsync(Message message,
        CancellationToken cancellationToken);

    Task SaveUserMessageAsync(UserMessage userMessage, CancellationToken cancellationToken);

    Task<List<GetChatResponse>> GetChat(Guid idUserSenderValidated, Guid idUserRecipientValidated,
        CancellationToken cancellationToken);

    Task<List<InteractedUsersResponse>> GetInteractedUsers(Guid idUser, CancellationToken cancellationToken);

    Task<GetUserDataResponse?> GetUserData
        (Guid idUser, CancellationToken cancellationToken);

    Task SaveConnectionData
        (HubConnection hubConnection, CancellationToken cancellationToken);

    Task<HubConnection?> FindHubConnectionByConnectionIdAndIdUser(Guid idUserValidated, string connectionId,
        CancellationToken cancellationToken);

    Task<bool> IsUserOnline(Guid idUserValidated, CancellationToken cancellationToken);
    Task DeleteConnectionData(Guid idUserValidated, string connectionId, CancellationToken cancellationToken);

    Task ExecuteMarkUserMessagesAsRead(Guid idUserSenderValidated, Guid idUserRecipientValidated,
        CancellationToken cancellationToken);

    Task<List<InteractedUsersResponse>> GetUnreadCountsForInteractedUsers(Guid idUser,
        List<InteractedUsersResponse> messagedUsers, CancellationToken cancellationToken);
}