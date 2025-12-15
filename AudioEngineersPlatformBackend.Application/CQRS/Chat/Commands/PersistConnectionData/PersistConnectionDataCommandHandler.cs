using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Domain.Entities;
using AudioEngineersPlatformBackend.Domain.Exceptions;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AudioEngineersPlatformBackend.Application.CQRS.Chat.Commands.PersistConnectionData;

public class
    PersistConnectionDataCommandHandler : IRequestHandler<PersistConnectionDataCommand,
    PersistConnectionDataCommandResult>
{
    private readonly ILogger<PersistConnectionDataCommandHandler> _logger;
    private readonly IValidator<PersistConnectionDataCommand> _inputValidator;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IChatRepository _chatRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PersistConnectionDataCommandHandler(
        ILogger<PersistConnectionDataCommandHandler> logger,
        IValidator<PersistConnectionDataCommand> inputValidator,
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

    public async Task<PersistConnectionDataCommandResult> Handle(
        PersistConnectionDataCommand persistConnectionDataCommand,
        CancellationToken cancellationToken
    )
    {
        // Validate the input.
        ValidationResult inputValidationResult = await _inputValidator.ValidateAsync
            (persistConnectionDataCommand, cancellationToken);

        if (!inputValidationResult.IsValid)
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: Input validation failed, data provided {@DataProvided}. {Errors}",
                nameof(PersistConnectionDataCommandHandler),
                nameof(Handle),
                persistConnectionDataCommand,
                inputValidationResult.Errors.FirstOrDefault()!.ErrorMessage
            );

            throw new ArgumentException($"{inputValidationResult.Errors.FirstOrDefault()}");
        }

        // See if the user exists.
        if (!await _userRepository.DoesUserExistByIdUserAsync(persistConnectionDataCommand.IdUser, cancellationToken))
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: User provided an IdUser {IdUser} which does not point towards any user.",
                nameof(PersistConnectionDataCommandHandler),
                nameof(Handle),
                persistConnectionDataCommand.IdUser
            );

            throw new BusinessRelatedException("User not found.");
        }

        // Persist all data, handling both cases (connecting and disconnecting).
        if (persistConnectionDataCommand.IsConnecting)
        {
            // Create a HubConnection and set its status to connected.
            HubConnection hubConnection = HubConnection.Create
                (persistConnectionDataCommand.IdUser, persistConnectionDataCommand.ConnectionId);

            // Save the entity.
            await _chatRepository.AddConnectionDataAsync(hubConnection, cancellationToken);

            // Persist all changes.
            await _unitOfWork.CompleteAsync(cancellationToken);
        }
        else
        {
            // Remove the connection from the DB, as the same connectionId (from SignalR) will never occur.
            await _chatRepository.ExecuteDeleteConnectionDataAsync
                (persistConnectionDataCommand.IdUser, persistConnectionDataCommand.ConnectionId, cancellationToken);
        }

        return new PersistConnectionDataCommandResult { Message = "Successfully persisted connection data." };
    }
}