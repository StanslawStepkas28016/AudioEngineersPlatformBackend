using AudioEngineersPlatformBackend.Application.Dtos;
using AudioEngineersPlatformBackend.Domain.Entities;

namespace AudioEngineersPlatformBackend.Application.Abstractions;

public interface IChatRepository
{
    Task SaveMessageAsync(
        Message message,
        CancellationToken cancellationToken
    );

    Task SaveUserMessageAsync(
        UserMessage userMessage,
        CancellationToken cancellationToken
    );

    Task<bool> IsUserOnlineAsync(
        Guid idUserValidated,
        CancellationToken cancellationToken
    );

    Task<PagedListDto<ChatMessageDto>> FindChatAsync(
        Guid idUserSender,
        Guid idUserRecipient,
        int page,
        int pageSize,
        CancellationToken cancellationToken
    );

    Task ExecuteMarkUserMessagesAsReadAsync(
        Guid idUserSender,
        Guid idUserRecipient,
        CancellationToken cancellationToken
    );

    Task<UserDataDto?> FindUserDataAsync(
        Guid idUser,
        CancellationToken cancellationToken
    );

    Task<List<InteractedUserDto>> FindInteractedUsersAsync(
        Guid idUser,
        CancellationToken cancellationToken
    );

    Task AddConnectionDataAsync(
        HubConnection hubConnection,
        CancellationToken cancellationToken
    );

    Task ExecuteDeleteConnectionDataAsync(
        Guid idUserValidated,
        string connectionId,
        CancellationToken cancellationToken
    );
}