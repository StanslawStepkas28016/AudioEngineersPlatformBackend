using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Application.Dtos;
using AudioEngineersPlatformBackend.Domain.Entities;
using AudioEngineersPlatformBackend.Infrastructure.Extensions;
using AudioEngineersPlatformBackend.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace AudioEngineersPlatformBackend.Infrastructure.Repositories;

public class ChatRepository : IChatRepository
{
    private readonly AudioEngineersPlatformDbContext _context;

    public ChatRepository(
        AudioEngineersPlatformDbContext context
    )
    {
        _context = context;
    }

    public async Task SaveMessageAsync(
        Message message,
        CancellationToken cancellationToken
    )
    {
        await _context
            .Messages
            .AddAsync(message, cancellationToken);
    }

    public async Task SaveUserMessageAsync(
        UserMessage userMessage,
        CancellationToken cancellationToken
    )
    {
        await _context
            .UserMessages
            .AddAsync(userMessage, cancellationToken);
    }

    public async Task<bool> IsUserOnlineAsync(
        Guid idUserValidated,
        CancellationToken cancellationToken
    )
    {
        return await _context
            .HubConnections
            .AnyAsync(hc => hc.IdUser == idUserValidated, cancellationToken);
    }

    public async Task<PagedListDto<ChatMessageDto>> FindChatAsync(
        Guid idUserSender,
        Guid idUserRecipient,
        int page,
        int pageSize,
        CancellationToken cancellationToken
    )
    {
        IQueryable<ChatMessageDto> chatMessagesQuery = _context
            .UserMessages
            .Where
            (um =>
                (um.IdUserSender == idUserSender && um.IdUserRecipient == idUserRecipient)
                ||
                (um.IdUserSender == idUserRecipient && um.IdUserRecipient == idUserSender)
            )
            .Select
            (um =>
                new ChatMessageDto
                {
                    IdMessage = um.IdMessage,
                    IdUserSender = um.IdUserSender,
                    IsRead = um.IsRead,
                    TextContent = um.Message.TextContent ?? "",
                    FileKey = um.Message.FileKey, // Can be empty.
                    FileName = um.Message.FileName ?? "",
                    FileUrl = "", // This will be initialized after calling an external dependency.
                    DateSent = um.Message.DateSent
                }
            )
            .OrderByDescending(um => um.DateSent)
            .AsQueryable();

        return await PagedListDtoExtension.ToPagedListAsync(chatMessagesQuery, page, pageSize, cancellationToken);
    }

    public async Task ExecuteMarkUserMessagesAsReadAsync(
        Guid idUserSender,
        Guid idUserRecipient,
        CancellationToken cancellationToken
    )
    {
        // We need to switch IdUserSender with IdUserRecipient,
        // as the recipient is the one to whom the messages were addressed to
        // (the one who is fetching chat messages).
        await _context
            .UserMessages
            .Where
            (um => um.IdUserSender == idUserRecipient
                   && um.IdUserRecipient == idUserSender
                   && !um.IsRead
            )
            .ExecuteUpdateAsync
            (
                pc => pc.SetProperty
                (
                    um => um.IsRead,
                    true
                ),
                cancellationToken
            );
    }

    public async Task<UserDataDto?> FindUserDataAsync(
        Guid idUser,
        CancellationToken cancellationToken
    )
    {
        return await _context
            .Users
            .Where(u => u.IdUser == idUser)
            .Select
            (u => new UserDataDto
                {
                    IdUser = idUser,
                    FirstName = u.FirstName,
                    LastName = u.LastName
                }
            )
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<InteractedUserDto>> FindInteractedUsersAsync(
        Guid idUser,
        CancellationToken cancellationToken
    )
    {
        // Fetch data of users that the user (identified by idUser) has messaged.
        List<InteractedUserDto> sentMessagesToList = await _context
            .UserMessages
            .Where(um => um.IdUserSender == idUser)
            .Select
            (um => new InteractedUserDto
                {
                    IdUser = um.IdUserRecipient,
                    FirstName = um.UserRecipient.FirstName,
                    LastName = um.UserRecipient.LastName
                }
            )
            .GroupBy(iud => iud.IdUser)
            .Select(g => g.First())
            .ToListAsync(cancellationToken);

        // Fetch data of users that the user (identified by idUser) has been messaged by.
        List<InteractedUserDto> gotMessagesFromList = await _context
            .UserMessages
            .Where(um => um.IdUserRecipient == idUser)
            .Select
            (um => new InteractedUserDto
                {
                    IdUser = um.IdUserSender,
                    FirstName = um.UserSender.FirstName,
                    LastName = um.UserSender.LastName
                }
            )
            .GroupBy(iud => iud.IdUser)
            .Select(g => g.First())
            .ToListAsync(cancellationToken);

        // Union both lists to avoid duplicates.
        List<InteractedUserDto> interactedUsersListUnion = sentMessagesToList
            .Union(gotMessagesFromList)
            .ToList();

        // Include counts for unread messages for users that have messaged the user (identified by idUser).
        foreach (InteractedUserDto interactedUserDto in interactedUsersListUnion)
        {
            interactedUserDto.UnreadCount =
                await _context
                    .UserMessages
                    .Where(u => u.IdUserSender == interactedUserDto.IdUser && u.IdUserRecipient == idUser && !u.IsRead)
                    .Select(u => u.IsRead)
                    .CountAsync(cancellationToken);
        }

        return interactedUsersListUnion;
    }

    public async Task AddConnectionDataAsync(
        HubConnection hubConnection,
        CancellationToken cancellationToken
    )
    {
        await _context
            .HubConnections
            .AddAsync(hubConnection, cancellationToken);
    }
    
    public async Task ExecuteDeleteConnectionDataAsync(
        Guid idUserValidated,
        string connectionId,
        CancellationToken cancellationToken
    )
    {
        await _context
            .HubConnections
            .Where(hc => hc.IdUser == idUserValidated && hc.ConnectionId == connectionId)
            .ExecuteDeleteAsync(cancellationToken);
    }
}