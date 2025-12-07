using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Domain.Entities;
using AudioEngineersPlatformBackend.Domain.Exceptions;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AudioEngineersPlatformBackend.Application.CQRS.Chat.Commands.SendFileMessage;

public class SendFileMessageCommandHandler : IRequestHandler<SendFileMessageCommand, SendFileMessageCommandResult>
{
    private readonly ILogger<SendFileMessageCommandHandler> _logger;
    private readonly IValidator<SendFileMessageCommand> _inputValidator;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IChatRepository _chatRepository;
    private readonly IS3Service _s3Service;
    private readonly IUnitOfWork _unitOfWork;

    public SendFileMessageCommandHandler(
        ILogger<SendFileMessageCommandHandler> logger,
        IValidator<SendFileMessageCommand> inputValidator,
        IMapper mapper,
        IUserRepository userRepository,
        IChatRepository chatRepository,
        IS3Service s3Service,
        IUnitOfWork unitOfWork
    )
    {
        _logger = logger;
        _inputValidator = inputValidator;
        _mapper = mapper;
        _userRepository = userRepository;
        _chatRepository = chatRepository;
        _s3Service = s3Service;
        _unitOfWork = unitOfWork;
    }

    public async Task<SendFileMessageCommandResult> Handle(
        SendFileMessageCommand sendFileMessageCommand,
        CancellationToken cancellationToken
    )
    {
        // Validate the input.
        ValidationResult inputValidationResult = await _inputValidator.ValidateAsync
            (sendFileMessageCommand, cancellationToken);

        if (!inputValidationResult.IsValid)
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: Input validation failed, data provided {@DataProvided}. {Errors}",
                nameof(SendFileMessageCommandHandler),
                nameof(Handle),
                sendFileMessageCommand,
                inputValidationResult.Errors.FirstOrDefault()!.ErrorMessage
            );

            throw new ArgumentException($"{inputValidationResult.Errors.FirstOrDefault()}");
        }

        // Ensure both users exist.
        if (!await _userRepository.DoesUserExistByIdUserAsync(sendFileMessageCommand.IdUserSender, cancellationToken)
            || !await _userRepository.DoesUserExistByIdUserAsync
                (sendFileMessageCommand.IdUserRecipient, cancellationToken)
           )
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: User provided at least one Id which does not point towards any user," +
                " IdUserSender {IdUserSender} and IdUserRecipient {IdUserRecipient}.",
                nameof(SendFileMessageCommandHandler),
                nameof(Handle),
                sendFileMessageCommand.IdUserSender,
                sendFileMessageCommand.IdUserRecipient
            );

            throw new BusinessRelatedException("Users not found.");
        }

        // Ensure users are not in the same role, meaning only Engineers can converse with Clients and vice versa.
        if (await _userRepository.AreUsersInTheSameRoleAsync
                (sendFileMessageCommand.IdUserSender, sendFileMessageCommand.IdUserRecipient, cancellationToken))
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: User with IdUserSender {IdUserSender} " +
                "tried to message a user with IdUserRecipient {IdUserRecipient}, who is in the same role as his.",
                nameof(SendFileMessageCommandHandler),
                nameof(Handle),
                sendFileMessageCommand.IdUserSender,
                sendFileMessageCommand.IdUserRecipient
            );

            throw new BusinessRelatedException("Users cannot be in the same role.");
        }

        // Create messages with corresponding fileKeys.
        Message fileMessage = Message.CreateFileMessage
            (sendFileMessageCommand.FileName, sendFileMessageCommand.FileKey);

        // Create a user message mapping.
        UserMessage userMessage = UserMessage.Create
            (sendFileMessageCommand.IdUserSender, sendFileMessageCommand.IdUserRecipient, fileMessage.IdMessage);

        // Get the senders info for client real-time notifications.
        var (senderFirstName, senderLastName) = await _userRepository.FindUserInfoByIdUserAsync
            (sendFileMessageCommand.IdUserSender, cancellationToken);

        // Get presigned url for reading/downloading the file for real-time notifications.
        string preSignedUrlForReadAsync = await _s3Service.GetPreSignedUrlForReadAsync
            ("files", fileMessage.FileName!, fileMessage.FileKey, cancellationToken);

        // Persist all changes.
        await _chatRepository.SaveMessageAsync(fileMessage, cancellationToken);
        await _chatRepository.SaveUserMessageAsync(userMessage, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return new SendFileMessageCommandResult
        {
            IdMessage = fileMessage.IdMessage,
            IdUserSender = userMessage.IdUserSender,
            SenderFirstName = senderFirstName,
            SenderLastName = senderLastName,
            TextContent = "",
            FileName = fileMessage.FileName!,
            FileKey = fileMessage.FileKey,
            FileUrl = preSignedUrlForReadAsync,
            DateSent = fileMessage.DateSent
        };
    }
}