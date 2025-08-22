using AudioEngineersPlatformBackend.Contracts.Message.GetMessagedUsers;
using AudioEngineersPlatformBackend.Contracts.Message.GetUserData;
using AudioEngineersPlatformBackend.Contracts.Message.GetUserMessages;
using AudioEngineersPlatformBackend.Domain.Entities;

namespace AudioEngineersPlatformBackend.Application.Abstractions;

public interface IMessagesRepository
{
    Task SaveMessageAsync(Message message,
        CancellationToken cancellationToken);

    Task SaveUserMessageAsync(UserMessage userMessage, CancellationToken cancellationToken);

    Task<List<GetMessageResponse>> GetUserMessages(Guid idUserSenderValidated, Guid idUserRecipientValidated,
        CancellationToken cancellationToken);

    Task<List<InteractedUsersResponse>> GetInteractedUsers(Guid idUser, CancellationToken cancellationToken);

    Task<GetUserDataResponse?> GetUserData
        (Guid idUser, CancellationToken cancellationToken);
}