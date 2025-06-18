using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Contracts.Chat.GetChat;
using AudioEngineersPlatformBackend.Contracts.Chat.GetMessagedUsers;
using AudioEngineersPlatformBackend.Contracts.Chat.GetUserData;
using AudioEngineersPlatformBackend.Domain.Entities;
using AudioEngineersPlatformBackend.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace AudioEngineersPlatformBackend.Infrastructure.Repositories;

public class ChatRepository : IChatRepository
{
    private readonly EngineersPlatformDbContext _context;

    public ChatRepository(EngineersPlatformDbContext context)
    {
        _context = context;
    }

    public async Task SaveMessageAsync(Message message,
        CancellationToken cancellationToken)
    {
        await _context
            .Messages
            .AddAsync(message, cancellationToken);
    }

    public async Task SaveUserMessageAsync(UserMessage userMessage, CancellationToken cancellationToken)
    {
        await _context
            .UserMessages
            .AddAsync(userMessage, cancellationToken);
    }

    public async Task<List<GetChatResponse>> GetChat(Guid idUserSenderValidated,
        Guid idUserRecipientValidated,
        CancellationToken cancellationToken)
    {
        return await _context
            .UserMessages
            .Where
            (um =>
                (um.IdUserSender == idUserSenderValidated && um.IdUserRecipient == idUserRecipientValidated)
                ||
                (um.IdUserSender == idUserRecipientValidated && um.IdUserRecipient == idUserSenderValidated)
            )
            .Select
            (um =>
                new GetChatResponse
                {
                    IdMessage = um.IdMessage,
                    IdUserSender = um.IdUserSender,
                    IsRead = um.IsRead,
                    TextContent = um.Message.TextContent,
                    FileKey = um.Message.FileKey, // Either this or FileUrl can be empty.
                    FileName = um.Message.FileName,
                    FileUrl = null, // This is specified after preSignedUrl creation in the ChatService.
                    DateSent = um.Message.DateSent,
                }
            )
            .OrderBy(um => um.DateSent)
            // .AsSplitQuery()
            .ToListAsync(cancellationToken);
    }

    public async Task<List<InteractedUsersResponse>> GetInteractedUsers(Guid idUser,
        CancellationToken cancellationToken)
    {
        // Fetch users who user identified by idUser has messaged.
        List<InteractedUsersResponse> messagedByUser = await _context
            .UserMessages
            .Where
                (um => um.IdUserSender == idUser)
            .Select
            (um =>
                new InteractedUsersResponse
                {
                    IdUser = um.IdUserRecipient,
                    FirstName = um.UserRecipient.FirstName,
                    LastName = um.UserRecipient.LastName,
                }
            )
            .GroupBy(x => x.IdUser)
            .Select(g => g.First())
            .ToListAsync(cancellationToken);

        // Fetch users who user identified by idUser has been messaged by.
        List<InteractedUsersResponse> userMessagedBy = await _context
            .UserMessages
            .Where
                (um => um.IdUserRecipient == idUser)
            .Select
            (um =>
                new InteractedUsersResponse
                {
                    IdUser = um.IdUserSender,
                    FirstName = um.UserSender.FirstName,
                    LastName = um.UserSender.LastName,
                }
            )
            .GroupBy(x => x.IdUser)
            .Select(g => g.First())
            .ToListAsync(cancellationToken);

        // Make sure we remove duplicates, by using Union() and overriding Equals in the selected DTO.
        return messagedByUser.Union(userMessagedBy).ToList();
    }

    public async Task<GetUserDataResponse?> GetUserData(Guid idUser, CancellationToken cancellationToken)
    {
        return await _context
            .Users
            .Where(u => u.IdUser == idUser)
            .Select
            (u => new GetUserDataResponse
                {
                    IdUser = idUser,
                    FirstName = u.FirstName,
                    LastName = u.LastName
                }
            )
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task SaveConnectionData(HubConnection hubConnection, CancellationToken cancellationToken)
    {
        await _context
            .HubConnections
            .AddAsync(hubConnection, cancellationToken);
    }

    public async Task<HubConnection?> FindHubConnectionByConnectionIdAndIdUser(Guid idUserValidated,
        string connectionId,
        CancellationToken cancellationToken)
    {
        return await _context
            .HubConnections
            .FirstOrDefaultAsync
                (hc => hc.IdUser == idUserValidated && hc.ConnectionId == connectionId, cancellationToken);
    }

    public async Task<bool> IsUserOnline(Guid idUserValidated, CancellationToken cancellationToken)
    {
        return await _context
            .HubConnections
            .AnyAsync(hc => hc.IdUser == idUserValidated, cancellationToken);
    }

    public async Task DeleteConnectionData(Guid idUserValidated, string connectionId,
        CancellationToken cancellationToken)
    {
        await _context
            .HubConnections
            .Where(hc => hc.IdUser == idUserValidated && hc.ConnectionId == connectionId)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task ExecuteMarkUserMessagesAsRead(Guid idUserSenderValidated, Guid idUserRecipientValidated,
        CancellationToken cancellationToken)
    {
        // Important: There will be no unitOfWork.CompleteAsync() call in the invoking service method
        // as we are using ExecuteUpdate, meaning all changes are immediate.
        await _context
            .UserMessages
            .Where
            (um => um.IdUserSender == idUserSenderValidated
                   && um.IdUserRecipient == idUserRecipientValidated
                   && !um.IsRead
            )
            .ExecuteUpdateAsync
            (
                pc => pc.SetProperty
                (
                    um => um.IsRead, true
                ),
                cancellationToken
            );
    }

    public async Task<List<InteractedUsersResponse>> GetUnreadCountsForInteractedUsers(Guid idUser,
        List<InteractedUsersResponse> messagedUsers, CancellationToken cancellationToken)
    {
        foreach (InteractedUsersResponse user in messagedUsers)
        {
            user.UnreadCount = await _context
                .UserMessages
                .Where(u => u.IdUserSender == user.IdUser && u.IdUserRecipient == idUser && !u.IsRead)
                .Select(u => !u.IsRead)
                .CountAsync(cancellationToken);
        }

        return messagedUsers;
    }
}