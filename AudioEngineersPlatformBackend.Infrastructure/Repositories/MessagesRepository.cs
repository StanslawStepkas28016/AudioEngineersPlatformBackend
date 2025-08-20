using AudioEngineersPlatformBackend.Application.Abstractions;
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
}