using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Domain.Entities;
using AudioEngineersPlatformBackend.Domain.Exceptions;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AudioEngineersPlatformBackend.Application.CQRS.Advert.Commands.CreateAdvert;

public class CreateAdvertCommandHandler : IRequestHandler<CreateAdvertCommand, CreateAdvertCommandResult>
{
    private readonly ILogger<CreateAdvertCommandHandler> _logger;
    private readonly IValidator<CreateAdvertCommand> _inputValidator;
    private readonly IMapper _mapper;
    private readonly IAdvertRepository _advertRepository;
    private readonly IUserRepository _userRepository;
    private readonly IS3Service _s3Service;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAdvertCommandHandler(
        ILogger<CreateAdvertCommandHandler> logger,
        IValidator<CreateAdvertCommand> inputValidator,
        IMapper mapper,
        IAdvertRepository advertRepository,
        IUserRepository userRepository,
        IS3Service s3Service,
        IUnitOfWork unitOfWork
    )
    {
        _logger = logger;
        _inputValidator = inputValidator;
        _mapper = mapper;
        _advertRepository = advertRepository;
        _userRepository = userRepository;
        _s3Service = s3Service;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateAdvertCommandResult> Handle(
        CreateAdvertCommand createAdvertCommand,
        CancellationToken cancellationToken
    )
    {
        // Validate the input.
        ValidationResult inputValidationResult = await _inputValidator.ValidateAsync
            (createAdvertCommand, cancellationToken);

        if (!inputValidationResult.IsValid)
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: Input validation failed, data provided {@DataProvided}. {Errors}",
                nameof(CreateAdvertCommandHandler),
                nameof(Handle),
                createAdvertCommand,
                inputValidationResult.Errors.FirstOrDefault()!.ErrorMessage
            );

            throw new ArgumentException($"{inputValidationResult.Errors.FirstOrDefault()}");
        }

        // Check if the user exists.
        if (!await _userRepository.DoesUserExistByIdUserAsync(createAdvertCommand.IdUser, cancellationToken))
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: User provided an IdUser from access token {Id} which does not point towards any user.",
                nameof(CreateAdvertCommandHandler),
                nameof(Handle),
                createAdvertCommand.IdUser
            );

            throw new BusinessRelatedException("User does not exist.");
        }

        // Check if the user has any adverts (limit is one).
        if (await _advertRepository.DoesUserHaveAnyAdvertByIdUserAsync(createAdvertCommand.IdUser, cancellationToken))
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: User already has an advert associated.",
                nameof(CreateAdvertCommandHandler),
                nameof(Handle)
            );

            throw new BusinessRelatedException("You already have an advert posted.");
        }

        // Check if the provided category name exists.
        AdvertCategory? advertCategory = await _advertRepository.FindAdvertCategoryByNameAsync
            (createAdvertCommand.CategoryName, cancellationToken);

        if (advertCategory == null)
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: User provided a category name {Name} which does not exist.",
                nameof(CreateAdvertCommandHandler),
                nameof(Handle),
                createAdvertCommand.CategoryName
            );

            throw new BusinessRelatedException("Category does not exist.");
        }

        // Create a new AdvertLog entity
        AdvertLog advertLog = AdvertLog.Create();

        // Upload the cover image file to S3 and get the key
        Guid imageKey = await _s3Service.UploadFileAsync
            ("images", createAdvertCommand.CoverImageFile, cancellationToken);

        // Create a new Advert entity
        Domain.Entities.Advert advert = Domain.Entities.Advert.Create
        (
            createAdvertCommand.Title,
            createAdvertCommand.Description,
            imageKey,
            createAdvertCommand.PortfolioUrl,
            createAdvertCommand.Price,
            createAdvertCommand.IdUser,
            advertCategory.IdAdvertCategory,
            advertLog.IdAdvertLog
        );

        // Add AdvertLog and Advert to the repository
        await _advertRepository.AddAdvertLogAsync(advertLog, cancellationToken);
        await _advertRepository.AddAdvertAsync(advert, cancellationToken);

        // Save all changes
        await _unitOfWork.CompleteAsync(cancellationToken);

        return new CreateAdvertCommandResult { IdUser = createAdvertCommand.IdUser, IdAdvert = advert.IdAdvert };
    }
}