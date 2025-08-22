using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Contracts.Message.GetMessagedUsers;
using AudioEngineersPlatformBackend.Contracts.Message.GetUserData;
using AudioEngineersPlatformBackend.Contracts.Message.GetUserMessages;
using AudioEngineersPlatformBackend.Domain.Entities;
using AudioEngineersPlatformBackend.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace AudioEngineersPlatformBackend.Infrastructure.Repositories;

public class MessagesRepository : IMessagesRepository
{
    private readonly EngineersPlatformDbContext _context;

    public MessagesRepository(EngineersPlatformDbContext context)
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

    public async Task<List<GetMessageResponse>> GetUserMessages(Guid idUserSenderValidated,
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
                new GetMessageResponse
                {
                    IdMessage = um.IdMessage,
                    IdUserSender = um.IdUserSender,
                    TextContent = um.Message.TextContent,
                    FileKey = um.Message.FileKey, // Either this or FileUrl can be empty.
                    FileUrl = null,
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
        return await _context
            .UserMessages
            .Where
            (um => um.IdUserSender == idUser
                   || um.IdUserRecipient == idUser
            )
            .Select
            (um =>
                new InteractedUsersResponse
                {
                    IdUser = (idUser == um.IdUserRecipient) ? um.IdUserSender : um.IdUserRecipient,
                    FirstName = (idUser == um.IdUserRecipient) ? um.UserSender.FirstName : um.UserRecipient.FirstName,
                    LastName = (idUser == um.IdUserRecipient) ? um.UserSender.LastName : um.UserRecipient.LastName,
                }
            )
            .GroupBy(x => x.IdUser)             
            .Select(g => g.First())
            .ToListAsync(cancellationToken);
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
}