using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Domain.Exceptions;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AudioEngineersPlatformBackend.Application.CQRS.Advert.Commands.DeleteAdvert;

public class DeleteAdvertCommandHandler : IRequestHandler<DeleteAdvertCommand, DeleteAdvertCommandResult>
{
    private readonly ILogger<DeleteAdvertCommandHandler> _logger;
    private readonly IValidator<DeleteAdvertCommand> _inputValidator;
    private readonly IMapper _mapper;
    private readonly IAdvertRepository _advertRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteAdvertCommandHandler(
        ILogger<DeleteAdvertCommandHandler> logger,
        IValidator<DeleteAdvertCommand> inputValidator,
        IMapper mapper,
        IAdvertRepository advertRepository,
        IUnitOfWork unitOfWork
    )
    {
        _logger = logger;
        _inputValidator = inputValidator;
        _mapper = mapper;
        _advertRepository = advertRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<DeleteAdvertCommandResult> Handle(
        DeleteAdvertCommand deleteAdvertCommand,
        CancellationToken cancellationToken
    )
    {
        // Validate the input.
        ValidationResult inputValidationResult = await _inputValidator.ValidateAsync
            (deleteAdvertCommand, cancellationToken);

        if (!inputValidationResult.IsValid)
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: Input validation failed, data provided {@DataProvided}. {Errors}",
                nameof(DeleteAdvertCommandHandler),
                nameof(Handle),
                deleteAdvertCommand,
                inputValidationResult.Errors.FirstOrDefault()!.ErrorMessage
            );

            throw new ArgumentException($"{inputValidationResult.Errors.FirstOrDefault()}");
        }

        // Check if the advert exists.
        Domain.Entities.Advert? advertAndAdvertLog = await _advertRepository
            .FindAdvertAndAdvertLogByIdUserAndIdAdvertAsync
                (deleteAdvertCommand.IdUser, deleteAdvertCommand.IdAdvert, cancellationToken);

        if (advertAndAdvertLog == null)
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: User provided an IdUser {IdUser} and IdAdvert {IdAdvert} which does not point towards any advert.",
                nameof(DeleteAdvertCommandHandler),
                nameof(Handle),
                deleteAdvertCommand.IdUser,
                deleteAdvertCommand.IdAdvert
            );

            throw new BusinessRelatedException("Advert not found.");
        }

        // Mark the advert as deleted.
        advertAndAdvertLog.AdvertLog.SetIsDeletedStatus(true);

        // Save the changes
        await _unitOfWork.CompleteAsync(cancellationToken);

        return new DeleteAdvertCommandResult { Message = "Advert successfully deleted." };
    }
}