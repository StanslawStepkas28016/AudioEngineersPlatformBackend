using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Domain.Exceptions;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AudioEngineersPlatformBackend.Application.CQRS.Advert.Commands.ChangeAdvertData;

public class ChangeAdvertDataCommandHandler : IRequestHandler<ChangeAdvertDataCommand, ChangeAdvertDataCommandResult>
{
    private readonly ILogger<ChangeAdvertDataCommandHandler> _logger;
    private readonly IValidator<ChangeAdvertDataCommand> _inputValidator;
    private readonly IMapper _mapper;
    private readonly IAdvertRepository _advertRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeAdvertDataCommandHandler(
        ILogger<ChangeAdvertDataCommandHandler> logger,
        IValidator<ChangeAdvertDataCommand> inputValidator,
        IMapper mapper,
        IAdvertRepository advertRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork
    )
    {
        _logger = logger;
        _inputValidator = inputValidator;
        _mapper = mapper;
        _advertRepository = advertRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ChangeAdvertDataCommandResult> Handle(
        ChangeAdvertDataCommand changeAdvertDataCommand,
        CancellationToken cancellationToken
    )
    {
        // Validate the input.
        ValidationResult inputValidationResult = await _inputValidator.ValidateAsync
            (changeAdvertDataCommand, cancellationToken);

        if (!inputValidationResult.IsValid)
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: Input validation failed, data provided {@DataProvided}. {Errors}",
                nameof(ChangeAdvertDataCommandHandler),
                nameof(Handle),
                changeAdvertDataCommand,
                inputValidationResult.Errors.FirstOrDefault()!.ErrorMessage
            );

            throw new BusinessRelatedException($"{inputValidationResult.Errors.FirstOrDefault()}");
        }

        // Get the advert data.
        Domain.Entities.Advert? advert = await _advertRepository.FindAdvertAndAdvertLogByIdUserAndIdAdvertAsync
            (changeAdvertDataCommand.IdUser, changeAdvertDataCommand.IdAdvert, cancellationToken);

        if (advert == null)
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: User provided an IdUser {IdUser} and IdAdvert {IdAdvert} which does not point towards any advert.",
                nameof(ChangeAdvertDataCommandHandler),
                nameof(Handle),
                changeAdvertDataCommand.IdUser,
                changeAdvertDataCommand.IdAdvert
            );

            throw new BusinessRelatedException("No advert found.");
        }

        // Ensure the advert has a correct status (is not deleted or inactive).
        advert.AdvertLog.EnsureCorrectStatus();

        // Perform an update on the adverts' data.
        advert
            .PartialUpdate
            (
                changeAdvertDataCommand.Title,
                changeAdvertDataCommand.Description,
                changeAdvertDataCommand.PortfolioUrl,
                changeAdvertDataCommand.Price
            );

        // Persist all changes.
        await _unitOfWork.CompleteAsync(cancellationToken);

        return new ChangeAdvertDataCommandResult
            { IdUser = advert.IdUser, IdAdvert = advert.IdAdvert };
    }
}