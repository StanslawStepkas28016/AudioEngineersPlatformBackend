using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Contracts.Message.GetUserMessages;
using AudioEngineersPlatformBackend.Contracts.Message.SaveAndSendTextMessage;
using AudioEngineersPlatformBackend.Domain.Entities;
using AudioEngineersPlatformBackend.Domain.ValueObjects;

namespace AudioEngineersPlatformBackend.Application.Services;

public class MessageService : IMessageService
{
    private readonly IMessagesRepository _messagesRepository;
    private readonly IS3Service _s3Service;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserUtil _currentUserUtil;

    public MessageService(IMessagesRepository messagesRepository,
        IS3Service s3Service,
        IUnitOfWork unitOfWork,
        ICurrentUserUtil currentUserUtil)
    {
        _messagesRepository = messagesRepository;
        _s3Service = s3Service;
        _unitOfWork = unitOfWork;
        _currentUserUtil = currentUserUtil;
    }

    public async Task SaveTextMessage(TextMessageRequest textMessageRequest,
        CancellationToken cancellationToken)
    {
        // Ensure valid input data.
        Guid idUserSenderValidated = new GuidVo(textMessageRequest.IdUserSender).Guid;
        Guid idUserRecipientValidated = new GuidVo(textMessageRequest.IdUserRecipient).Guid;

        // Ensure user is sending  
        if (idUserSenderValidated != _currentUserUtil.IdUser && !_currentUserUtil.IsAdministrator)
        {
            throw new UnauthorizedAccessException($"You are trying to impersonate an existing {nameof(User)}.");
        }

        // Create a text message.
        Message textMessage = Message.CreateTextMessage(textMessageRequest.TextContent);

        // Create a user message mapping.
        UserMessage userMessage = UserMessage.Create
            (idUserSenderValidated, idUserRecipientValidated, textMessage.IdMessage);

        // Persist all changes.
        await _messagesRepository.SaveMessageAsync(textMessage, cancellationToken);
        await _messagesRepository.SaveUserMessageAsync(userMessage, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);
    }

    // public async Task SaveFileMessageMessage(Guid idUserRecipient, TextMessageRequest textMessageRequest,
    //     CancellationToken cancellationToken)

    public async Task<List<GetMessageResponse>> GetUserMessages(Guid idUserSender, Guid idUserRecipient,
        CancellationToken cancellationToken)
    {
        // Ensure valid input data.
        Guid idUserSenderValidated = new GuidVo(idUserSender).Guid;
        Guid idUserRecipientValidated = new GuidVo(idUserRecipient).Guid;

        // Ensure user cannot access their own conversation, as this would result in a db error.
        if (idUserSenderValidated == idUserRecipientValidated)
        {
            throw new Exception($"You cannot read your own messages.");
        }

        // Ensure the user trying to access messages is actually him.
        if (_currentUserUtil.IdUser != idUserSender && !_currentUserUtil.IsAdministrator)
        {
            throw new UnauthorizedAccessException($"You cannot access else's messages.");
        }

        // Get user messages (both text messages, and file messages which will
        // have their presigned URL's generated).
        List<GetMessageResponse> messages = await _messagesRepository.GetUserMessages
            (idUserSenderValidated, idUserRecipientValidated, cancellationToken);

        // Ensure users have a conversation.
        if (messages.Count == 0)
        {
            throw new Exception
            (
                $"{nameof(User)} with specified {nameof(idUserSender)} has no messages with a {nameof(User)} of specified {nameof(idUserRecipient)}."
            );
        }

        // Generate presigned URL's for files via AWS S3.
        foreach (var message in messages.Where(message => message.FileKey != Guid.Empty))
        {
            message.FileUrl = await _s3Service.GetPreSignedUrlAsync(message.FileKey, cancellationToken);
        }

        return messages;
    }
}