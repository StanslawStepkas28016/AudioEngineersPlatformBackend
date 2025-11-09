using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Application.Dtos;
using AudioEngineersPlatformBackend.Domain.Exceptions;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AudioEngineersPlatformBackend.Application.CQRS.Chat.Queries.GetChat;

public class GetChatQueryHandler : IRequestHandler<GetChatQuery, GetChatQueryResult>
{
    private readonly ILogger<GetChatQueryHandler> _logger;
    private readonly IValidator<GetChatQuery> _inputValidator;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IChatRepository _chatRepository;
    private readonly IS3Service _s3Service;

    public GetChatQueryHandler(
        ILogger<GetChatQueryHandler> logger,
        IValidator<GetChatQuery> inputValidator,
        IMapper mapper,
        IUserRepository userRepository,
        IChatRepository chatRepository,
        IS3Service s3Service
    )
    {
        _logger = logger;
        _inputValidator = inputValidator;
        _mapper = mapper;
        _userRepository = userRepository;
        _chatRepository = chatRepository;
        _s3Service = s3Service;
    }

    public async Task<GetChatQueryResult> Handle(
        GetChatQuery getChatQuery,
        CancellationToken cancellationToken
    )
    {
        // Validate the input.
        ValidationResult inputValidationResult = await _inputValidator.ValidateAsync
            (getChatQuery, cancellationToken);

        if (!inputValidationResult.IsValid)
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: Input validation failed, data provided {@DataProvided}. {Errors}",
                nameof(GetChatQueryHandler),
                nameof(Handle),
                getChatQuery,
                inputValidationResult.Errors.FirstOrDefault()!.ErrorMessage
            );

            throw new ArgumentException($"{inputValidationResult.Errors.FirstOrDefault()}");
        }

        // Ensure both users exist.
        if (!await _userRepository.DoesUserExistByIdUserAsync(getChatQuery.IdUserSender, cancellationToken)
            || !await _userRepository.DoesUserExistByIdUserAsync
                (getChatQuery.IdUserRecipient, cancellationToken)
           )
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: User provided at least one Id which does not point towards any user," +
                " IdUserSender {IdUserSender} and IdUserRecipient {IdUserRecipient}.",
                nameof(GetChatQueryHandler),
                nameof(Handle),
                getChatQuery.IdUserSender,
                getChatQuery.IdUserRecipient
            );

            throw new BusinessRelatedException("Users not found.");
        }

        // Get the chat data.
        PagedListDto<ChatMessageDto> pagedChatMessages = await _chatRepository.FindChatAsync
        (
            getChatQuery.IdUserSender,
            getChatQuery.IdUserRecipient,
            getChatQuery.Page,
            getChatQuery.PageSize,
            cancellationToken
        );

        // Reverse the order of the items for chat like pagination on the client side.
        pagedChatMessages.Items.Reverse();

        // Mark fetched messages from recipient as read.
        await _chatRepository.ExecuteMarkUserMessagesAsReadAsync
            (getChatQuery.IdUserSender, getChatQuery.IdUserRecipient, cancellationToken);

        // Generate presigned URL's for files via AWS S3 for messages with non-empty fileKeys (those are file-messages). 
        foreach (ChatMessageDto chatMessageDto in pagedChatMessages.Items.Where
                     (message => message.FileKey != Guid.Empty))
        {
            chatMessageDto.FileUrl = await _s3Service.GetPreSignedUrlForReadAsync
                ("files", chatMessageDto.FileName, chatMessageDto.FileKey, cancellationToken);
        }

        // Map to result.
        GetChatQueryResult result = _mapper.Map<PagedListDto<ChatMessageDto>, GetChatQueryResult>
            (pagedChatMessages);

        return result;
    }
}