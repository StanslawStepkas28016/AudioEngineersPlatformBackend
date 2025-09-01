using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Contracts.Chat.GetChat;
using AudioEngineersPlatformBackend.Contracts.Chat.GetMessagedUsers;
using AudioEngineersPlatformBackend.Contracts.Chat.GetPresignedUrlForUpload;
using AudioEngineersPlatformBackend.Contracts.Chat.GetUserData;
using AudioEngineersPlatformBackend.Contracts.Chat.PersistSessionData;
using AudioEngineersPlatformBackend.Contracts.Chat.SaveAndSendFileMessage;
using AudioEngineersPlatformBackend.Contracts.Chat.SaveAndSendGeneralMessage;
using AudioEngineersPlatformBackend.Contracts.Chat.SaveAndSendTextMessage;
using AudioEngineersPlatformBackend.Domain.Entities;
using AudioEngineersPlatformBackend.Domain.ValueObjects;

namespace AudioEngineersPlatformBackend.Application.Services;

public class ChatService : IChatService
{
    private readonly IChatRepository _chatRepository;
    private readonly IUserRepository _userRepository;
    private readonly IS3Service _s3Service;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserUtil _currentUserUtil;

    public ChatService(IChatRepository chatRepository,
        IUserRepository userRepository,
        IS3Service s3Service,
        IUnitOfWork unitOfWork,
        ICurrentUserUtil currentUserUtil)
    {
        _chatRepository = chatRepository;
        _userRepository = userRepository;
        _s3Service = s3Service;
        _unitOfWork = unitOfWork;
        _currentUserUtil = currentUserUtil;
    }

    public async Task<MessageResponse> SaveTextMessage(Guid idUserSender, TextMessageRequest textMessageRequest,
        CancellationToken cancellationToken)
    {
        // Ensure valid input data.
        Guid idUserSenderValidated = new GuidVo(idUserSender).Guid;
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
                $"{nameof(User)} with specified {nameof(idUserSender)} " +
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
        await _chatRepository.SaveMessageAsync(textMessage, cancellationToken);
        await _chatRepository.SaveUserMessageAsync(userMessage, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        // Get the senders info for client real-time notifications.
        var (senderFirstName, senderLastName) = await _userRepository.FindUserInfoByIdUserAsync
            (idUserSenderValidated, cancellationToken);

        return new MessageResponse
        {
            IdMessage = textMessage.IdMessage,
            IdUserSender = userMessage.IdUserSender,
            SenderFirstName = senderFirstName,
            SenderLastName = senderLastName,
            TextContent = textMessage.TextContent,
            FileName = "",
            FileUrl = "",
            FileKey = Guid.Empty,
            DateSent = textMessage.DateSent
        };
    }

    public async Task<MessageResponse> SaveFileMessage(Guid idUserSender, FileMessageRequest fileMessageRequest,
        CancellationToken cancellationToken)
    {
        // Ensure valid input data.
        Guid idUserSenderValidated = new GuidVo(idUserSender).Guid;
        Guid idUserRecipientValidated = new GuidVo(fileMessageRequest.IdUserRecipient).Guid;

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
                $"{nameof(User)} with specified {nameof(idUserSender)} " +
                $"or {nameof(fileMessageRequest.IdUserRecipient)} does not exist."
            );
        }

        // Ensure user can actually send a message to other user (they need to be in different roles).
        if (await _userRepository.AreInTheSameRole(idUserSenderValidated, idUserRecipientValidated, cancellationToken))
        {
            throw new Exception($"You cannot send a {nameof(Message)} to a {nameof(User)} of the same {nameof(Role)}.");
        }

        // Create messages with corresponding fileKeys.
        Message fileMessage = Message.CreateFileMessage(fileMessageRequest.FileName, fileMessageRequest.FileKey);

        // Create UserMessage mappings for messages.
        UserMessage userMessage = UserMessage.Create
            (idUserSenderValidated, idUserRecipientValidated, fileMessage.IdMessage);

        // Persist all changes.
        await _chatRepository.SaveMessageAsync(fileMessage, cancellationToken);
        await _chatRepository.SaveUserMessageAsync(userMessage, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        // Get the senders info for client real-time notifications.
        var (senderFirstName, senderLastName) = await _userRepository.FindUserInfoByIdUserAsync
            (idUserSenderValidated, cancellationToken);

        return new MessageResponse
        {
            IdMessage = fileMessage.IdMessage,
            IdUserSender = userMessage.IdUserSender,
            SenderFirstName = senderFirstName,
            SenderLastName = senderLastName,
            TextContent = null,
            FileName = fileMessage.FileName!,
            FileKey = fileMessageRequest.FileKey,
            FileUrl = await _s3Service.GetPreSignedUrlForReadAsync
                ("files", fileMessage.FileName!, fileMessageRequest.FileKey, cancellationToken),
            DateSent = fileMessage.DateSent
        };
    }

    public async Task<GetPresignedUrlForUploadResponse> GetPresignedUrlForUpload(string folder, string fileName,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentException($"{nameof(fileName)} cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(folder))
        {
            throw new ArgumentException($"{nameof(folder)} cannot be empty.");
        }

        var fileKey = Guid.NewGuid();

        string preSignedUrl = await _s3Service.GetPreSignedUrlForUploadAsync
        (
            folder,
            fileKey,
            fileName,
            cancellationToken
        );

        return new GetPresignedUrlForUploadResponse
        {
            FileKey = fileKey,
            PreSignedUrlForUpload = preSignedUrl,
        };
    }

    public async Task<List<GetChatResponse>> GetChat(Guid idUserSender, Guid idUserRecipient,
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
        List<GetChatResponse> messages = await _chatRepository.GetChat
            (idUserSenderValidated, idUserRecipientValidated, cancellationToken);

        // Mark unread chat messages as read. We assume that when the user fetches
        // a conversation (sender - him, recipient - other) he is going to mark all (recipient - other)
        // messages as read.

        // Important: We need to switch the id's, as all the messages send from (other)
        // are now being marked as read.
        await _chatRepository.ExecuteMarkUserMessagesAsRead
            (idUserRecipientValidated, idUserSenderValidated, cancellationToken);

        // Generate presigned URL's for files via AWS S3 for messages with non-empty fileKeys (those are fileMessages). 
        foreach (var message in messages.Where(message => message.FileKey != Guid.Empty))
        {
            message.FileUrl = await _s3Service.GetPreSignedUrlForReadAsync
                ("files", message.FileName!, message.FileKey, cancellationToken);
        }

        return messages;
    }

    public async Task<List<InteractedUsersResponse>> GetInteractedUsers(Guid idUser,
        CancellationToken cancellationToken)
    {
        // Validate the input.
        Guid idUserValidated = new GuidVo(idUser).Guid;

        // Get interacted users not including their unread counts.
        List<InteractedUsersResponse> messagedUsers = await _chatRepository.GetInteractedUsers
            (idUserValidated, cancellationToken);

        // Include unread counts for interacted users.
        List<InteractedUsersResponse> messagedUsersWithUnreadCounts =
            await _chatRepository.GetUnreadCountsForInteractedUsers(idUser, messagedUsers, cancellationToken);

        return messagedUsersWithUnreadCounts;
    }

    public async Task<GetUserDataResponse> GetUserData(Guid idUser, CancellationToken cancellationToken)
    {
        // Validate the input.
        Guid idUserValidated = new GuidVo(idUser).Guid;

        // Find the associated data.
        GetUserDataResponse? userData = await _chatRepository.GetUserData(idUserValidated, cancellationToken);

        if (userData == null)
        {
            throw new ArgumentException($"{nameof(User)} with the provided {nameof(idUser)} does not exist.");
        }

        return userData;
    }

    public async Task PersistConnectionData(Guid idUser, PersistConnectionDataRequest persistConnectionDataRequest,
        CancellationToken cancellationToken)
    {
        // Validate the input.
        Guid idUserValidated = new GuidVo(idUser).Guid;

        // Check if the user exists.
        if (!await _userRepository.DoesUserExistByIdUserAsync(idUserValidated, cancellationToken))
        {
            throw new ArgumentException($"{nameof(User)} with the provided {nameof(idUser)} does not exist.");
        }

        // Handle both cases (connecting and disconnecting).
        if (persistConnectionDataRequest.IsConnecting)
        {
            // Create a HubConnection and set its status to connected.
            HubConnection hubConnection = HubConnection.Create
                (idUserValidated, persistConnectionDataRequest.ConnectionId);

            // Save the entity.
            await _chatRepository.SaveConnectionData(hubConnection, cancellationToken);
        }
        else
        {
            // Remove the connection from the DB, as the same connectionId will never occur.
            await _chatRepository.DeleteConnectionData
                (idUserValidated, persistConnectionDataRequest.ConnectionId, cancellationToken);
        }

        // Persist all changes.
        await _unitOfWork.CompleteAsync(cancellationToken);
    }

    public async Task<bool> GetUserOnlineStatus(Guid idUser, CancellationToken cancellationToken)
    {
        // Validate the input.
        Guid idUserValidated = new GuidVo(idUser).Guid;

        // See if the user exists.
        if (!await _userRepository.DoesUserExistByIdUserAsync(idUser, cancellationToken))
        {
            throw new Exception($"{nameof(User)} with the provided {nameof(idUser)} does not exist.");
        }

        // Find if the user is online.
        bool isOnline = await _chatRepository
            .IsUserOnline(idUserValidated, cancellationToken);

        return isOnline;
    }
}