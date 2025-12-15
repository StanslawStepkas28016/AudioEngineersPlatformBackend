using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Domain.Entities;
using AudioEngineersPlatformBackend.Domain.Exceptions;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AudioEngineersPlatformBackend.Application.CQRS.Chat.Commands.SendTextMessage;

public class SendTextMessageCommandHandler : IRequestHandler<SendTextMessageCommand, SendTextMessageCommandResult>
{
    private readonly ILogger<SendTextMessageCommandHandler> _logger;
    private readonly IValidator<SendTextMessageCommand> _inputValidator;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IChatRepository _chatRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SendTextMessageCommandHandler(
        ILogger<SendTextMessageCommandHandler> logger,
        IValidator<SendTextMessageCommand> inputValidator,
        IMapper mapper,
        IUserRepository userRepository,
        IChatRepository chatRepository,
        IUnitOfWork unitOfWork
    )
    {
        _logger = logger;
        _inputValidator = inputValidator;
        _mapper = mapper;
        _userRepository = userRepository;
        _chatRepository = chatRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<SendTextMessageCommandResult> Handle(
        SendTextMessageCommand sendTextMessageCommand,
        CancellationToken cancellationToken
    )
    {
        // Validate the input.
        ValidationResult inputValidationResult = await _inputValidator.ValidateAsync
            (sendTextMessageCommand, cancellationToken);

        if (!inputValidationResult.IsValid)
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: Input validation failed, data provided {@DataProvided}. {Errors}",
                nameof(SendTextMessageCommandHandler),
                nameof(Handle),
                sendTextMessageCommand,
                inputValidationResult.Errors.FirstOrDefault()!.ErrorMessage
            );

            throw new ArgumentException($"{inputValidationResult.Errors.FirstOrDefault()}");
        }

        // Ensure both users exist.
        if (!await _userRepository.DoesUserExistByIdUserAsync(sendTextMessageCommand.IdUserSender, cancellationToken)
            || !await _userRepository.DoesUserExistByIdUserAsync
                (sendTextMessageCommand.IdUserRecipient, cancellationToken)
           )
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: User provided at least one Id which does not point towards any user," +
                " IdUserSender {IdUserSender} and IdUserRecipient {IdUserRecipient}.",
                nameof(SendTextMessageCommandHandler),
                nameof(Handle),
                sendTextMessageCommand.IdUserSender,
                sendTextMessageCommand.IdUserRecipient
            );

            throw new BusinessRelatedException("Users not found.");
        }

        // Ensure users are not in the same role, meaning only Engineers can converse with Clients and vice versa.
        if (await _userRepository.AreUsersInTheSameRoleAsync
                (sendTextMessageCommand.IdUserSender, sendTextMessageCommand.IdUserRecipient, cancellationToken))
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: User with IdUserSender {IdUserSender} " +
                "tried to message a user with IdUserRecipient {IdUserRecipient}, who is in the same role as his.",
                nameof(SendTextMessageCommandHandler),
                nameof(Handle),
                sendTextMessageCommand.IdUserSender,
                sendTextMessageCommand.IdUserRecipient
            );

            throw new BusinessRelatedException("Users cannot be in the same role.");
        }

        // Create a text message.
        Message textMessage = Message.CreateTextMessage(sendTextMessageCommand.TextContent);

        // Create a user message mapping.
        UserMessage userMessage = UserMessage.Create
            (sendTextMessageCommand.IdUserSender, sendTextMessageCommand.IdUserRecipient, textMessage.IdMessage);

        // Get the senders info for client real-time notifications.
        var (senderFirstName, senderLastName) = await _userRepository.FindUserInfoByIdUserAsync
            (sendTextMessageCommand.IdUserSender, cancellationToken);
        
        // Persist all changes.
        await _chatRepository.SaveMessageAsync(textMessage, cancellationToken);
        await _chatRepository.SaveUserMessageAsync(userMessage, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);
        
        return new SendTextMessageCommandResult
        {
            IdMessage = textMessage.IdMessage,
            IdUserSender = userMessage.IdUserSender,
            SenderFirstName = senderFirstName,
            SenderLastName = senderLastName,
            TextContent = textMessage.TextContent!,
            FileName = "",
            FileUrl = "",
            FileKey = Guid.Empty,
            DateSent = textMessage.DateSent
        };
    }
}