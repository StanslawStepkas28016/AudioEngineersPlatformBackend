using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Domain.Entities;
using AudioEngineersPlatformBackend.Domain.Exceptions;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AudioEngineersPlatformBackend.Application.CQRS.Advert.Commands.AddReview;

public class AddReviewCommandHandler : IRequestHandler<AddReviewCommand, AddReviewCommandResult>
{
    private readonly ILogger<AddReviewCommandHandler> _logger;
    private readonly IValidator<AddReviewCommand> _inputValidator;
    private readonly IMapper _mapper;
    private readonly IAdvertRepository _advertRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddReviewCommandHandler(
        ILogger<AddReviewCommandHandler> logger,
        IValidator<AddReviewCommand> inputValidator,
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

    public async Task<AddReviewCommandResult> Handle(
        AddReviewCommand addReviewCommand,
        CancellationToken cancellationToken
    )
    {
        // Validate the input.
        ValidationResult inputValidationResult = await _inputValidator.ValidateAsync
            (addReviewCommand, cancellationToken);

        if (!inputValidationResult.IsValid)
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: Input validation failed, data provided {@DataProvided}. {Errors}",
                nameof(AddReviewCommandHandler),
                nameof(Handle),
                addReviewCommand,
                inputValidationResult.Errors.FirstOrDefault()!.ErrorMessage
            );

            throw new ArgumentException($"{inputValidationResult.Errors.FirstOrDefault()}");
        }

        // Check if the user exists.
        if (!await _userRepository.DoesUserExistByIdUserAsync(addReviewCommand.IdUserReviewer, cancellationToken))
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: User provided an IdUser {IdUser} which does not point to any user.",
                nameof(AddReviewCommandHandler),
                nameof(Handle),
                addReviewCommand.IdUserReviewer
            );

            throw new BusinessRelatedException("User not found.");
        }

        // Check if the advert exists.
        if (!await _advertRepository.DoesAdvertExistByIdAdvertAsync(addReviewCommand.IdAdvert, cancellationToken))
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: User provided an IdAdvert {IdAdvert} which does not exist.",
                nameof(AddReviewCommandHandler),
                nameof(Handle),
                addReviewCommand.IdAdvert
            );

            throw new BusinessRelatedException("Advert not found.");
        }

        // Check if the user has already posted a review under the requested advert.
        if (await _advertRepository.DoesUserHaveAReviewPostedAlreadyByIdUserReviewerAndIdAdvert
                (addReviewCommand.IdUserReviewer, addReviewCommand.IdAdvert, cancellationToken))
        {
            _logger.LogError
            (
                "Error from Class {ClassName}, Method {MethodName}: User with {IdUser} tried to post a review under an advert with IdAdvert {IdAdvert} that they have already posted before.",
                nameof(AddReviewCommandHandler),
                nameof(Handle),
                addReviewCommand.IdUserReviewer,
                addReviewCommand.IdAdvert
            );

            throw new BusinessRelatedException("You have already posted a review.");
        }

        // Create a ReviewLog
        ReviewLog reviewLog = ReviewLog.Create();

        // Create a Review
        Review review = Review.Create
        (
            addReviewCommand.IdAdvert,
            reviewLog.IdReviewLog,
            addReviewCommand.IdUserReviewer,
            addReviewCommand.Content,
            addReviewCommand.SatisfactionLevel
        );

        // Persist the data
        await _advertRepository.AddReviewLogAsync(reviewLog, cancellationToken);
        await _advertRepository.AddReviewAsync(review, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return new AddReviewCommandResult { Message = "Review created successfully." };
    }
}