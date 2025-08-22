using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Contracts.Message.GetMessagedUsers;
using AudioEngineersPlatformBackend.Contracts.Message.GetUserData;
using AudioEngineersPlatformBackend.Contracts.Message.GetUserMessages;
using AudioEngineersPlatformBackend.Contracts.Message.SaveAndSendTextMessage;
using AudioEngineersPlatformBackend.Domain.Entities;
using AudioEngineersPlatformBackend.Domain.ValueObjects;

namespace AudioEngineersPlatformBackend.Application.Services;

public class MessageService : IMessageService
{
    private readonly IMessagesRepository _messagesRepository;
    private readonly IUserRepository _userRepository;
    private readonly IS3Service _s3Service;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserUtil _currentUserUtil;

    public MessageService(IMessagesRepository messagesRepository,
        IUserRepository userRepository,
        IS3Service s3Service,
        IUnitOfWork unitOfWork,
        ICurrentUserUtil currentUserUtil)
    {
        _messagesRepository = messagesRepository;
        _userRepository = userRepository;
        _s3Service = s3Service;
        _unitOfWork = unitOfWork;
        _currentUserUtil = currentUserUtil;
    }

    public async Task<TextMessageResponse> SaveTextMessage(TextMessageRequest textMessageRequest,
        CancellationToken cancellationToken)
    {
        // Ensure valid input data.
        Guid idUserSenderValidated = new GuidVo(textMessageRequest.IdUserSender).Guid;
        Guid idUserRecipientValidated = new GuidVo(textMessageRequest.IdUserRecipient).Guid;
        
        // Ensure user is sending a not trying to impersonate anyone else.  
        if (idUserSenderValidated != _currentUserUtil.IdUser && !_currentUserUtil.IsAdministrator)
        {
            throw new UnauthorizedAccessException($"You are trying to impersonate an existing {nameof(User)}.");
        }
        
        // Ensure both users exist.
        if (!await _userRepository.DoesUserExistByIdUserAsync(idUserSenderValidated, cancellationToken)
            || !await _userRepository.DoesUserExistByIdUserAsync(idUserRecipientValidated, cancellationToken))
        {
            throw new Exception
            (
                $"{nameof(User)} with specified {nameof(textMessageRequest.IdUserSender)} " +
                $"or {nameof(textMessageRequest.IdUserRecipient)} does not exist."
            );
        }

        // Ensure user can actually send a message to other user (they need to be in different roles).
        if (await _userRepository.AreInTheSameRole(idUserSenderValidated, idUserRecipientValidated, cancellationToken))
        {
            throw new Exception($"You cannot send a {nameof(Message)} to a {nameof(User)} of the same {nameof(Role)}.");
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

        return new TextMessageResponse
        {
            IdMessage = textMessage.IdMessage,
            IdUserSender = userMessage.IdUserSender,
            TextContent = textMessage.TextContent,
            FileUrl = "",
            DateSent = textMessage.DateSent
        };
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
        
        // Ensure both users exist.
        if (!await _userRepository.DoesUserExistByIdUserAsync(idUserSenderValidated, cancellationToken)
            || !await _userRepository.DoesUserExistByIdUserAsync(idUserRecipientValidated, cancellationToken))
        {
            throw new Exception
            (
                $"{nameof(User)} with specified {nameof(idUserSender)} " +
                $"or {nameof(idUserRecipient)} does not exist."
            );
        }
        
        // Get user messages (both text messages, and file messages which will
        // have their presigned URL's generated).
        List<GetMessageResponse> messages = await _messagesRepository.GetUserMessages
            (idUserSenderValidated, idUserRecipientValidated, cancellationToken);
        
        // Generate presigned URL's for files via AWS S3.
        foreach (var message in messages.Where(message => message.FileKey != Guid.Empty))
        {
            message.FileUrl = await _s3Service.GetPreSignedUrlAsync(message.FileKey, cancellationToken);
        }

        return messages;
    }

    public async Task<List<InteractedUsersResponse>> GetInteractedUsers(Guid idUser,
        CancellationToken cancellationToken)
    {
        Guid idUserValidated = new GuidVo(idUser).Guid;

        List<InteractedUsersResponse> messagedUsers = await _messagesRepository.GetInteractedUsers
            (idUserValidated, cancellationToken);

        return messagedUsers;
    }

    public async Task<GetUserDataResponse> GetUserData(Guid idUser, CancellationToken cancellationToken)
    {
        // Validate the input.
        Guid idUserValidated = new GuidVo(idUser).Guid;

        // Find the associated data.
        GetUserDataResponse? userData = await _messagesRepository.GetUserData(idUserValidated, cancellationToken);

        if (userData == null)
        {
            throw new ArgumentException($"{nameof(User)} with the provided {nameof(idUser)} does not exist.");
        }

        return userData;
    }
}