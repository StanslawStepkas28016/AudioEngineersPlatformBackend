using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Application.Dtos;
using AudioEngineersPlatformBackend.Contracts.Advert.AddReview;
using AudioEngineersPlatformBackend.Contracts.Advert.ChangeAdverData;
using AudioEngineersPlatformBackend.Contracts.Advert.CreateAdvert;
using AudioEngineersPlatformBackend.Contracts.Advert.GetAdvertDetails;
using AudioEngineersPlatformBackend.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace AudioEngineersPlatformBackend.Application.Services;

public class AdvertService : IAdvertService
{
    private readonly IAdvertRepository _advertRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IS3Service _s3Service;
    private readonly ICurrentUserService _currentUserService;

    public AdvertService(IAdvertRepository advertRepository, IUnitOfWork unitOfWork, IS3Service s3Service,
        ICurrentUserService currentUserService, IUserRepository userRepository)
    {
        _advertRepository = advertRepository;
        _unitOfWork = unitOfWork;
        _s3Service = s3Service;
        _currentUserService = currentUserService;
        _userRepository = userRepository;
    }

    public async Task<CreateAdvertResponse> CreateAdvert(CreateAdvertRequest createAdvertRequest,
        CancellationToken cancellationToken)
    {
        // Validate the user from the request
        if (createAdvertRequest.IdUser == Guid.Empty)
        {
            throw new ArgumentException("User ID cannot be empty.");
        }

        // Check if the user exists
        var findUserByIdUser = await _userRepository.DoesUserExistByIdUser(createAdvertRequest.IdUser, cancellationToken);

        if (!findUserByIdUser)
        {
            throw new ArgumentException("IdUser cannot be empty.", nameof(createAdvertRequest.IdUser));
        }

        // Check if there is already an advert posted by the user
        Advert? advertByIdUser =
            await _advertRepository.GetActiveAndNonDeletedAdvertByIdUser(createAdvertRequest.IdUser, cancellationToken);

        if (advertByIdUser != null)
        {
            throw new ArgumentException("User already has an advert posted.");
        }

        // Check if the provided category name exists
        if (string.IsNullOrWhiteSpace(createAdvertRequest.CategoryName))
        {
            throw new ArgumentException("Category name cannot be null or whitespace.",
                nameof(createAdvertRequest.CategoryName));
        }

        AdvertCategory? advertCategory = await _advertRepository
            .GetAdvertCategoryByCategoryName(createAdvertRequest.CategoryName, cancellationToken);

        if (advertCategory == null)
        {
            throw new Exception("Category not found.");
        }

        // Create a new AdvertLog entity
        AdvertLog advertLog = AdvertLog.Create();

        // Upload the cover image file to S3 and get the key
        Guid imageKey = await _s3Service.TryUploadFileAsync(createAdvertRequest.CoverImageFile, cancellationToken);

        // Create a new Advert entity
        Advert advert = Advert.Create(
            createAdvertRequest.Title,
            createAdvertRequest.Description,
            imageKey,
            createAdvertRequest.PortfolioUrl,
            createAdvertRequest.Price,
            createAdvertRequest.IdUser,
            advertCategory.IdAdvertCategory,
            advertLog.IdAdvertLog
        );

        // Add AdvertLog and Advert to the repository
        await _advertRepository.AddAdvertLog(advertLog, cancellationToken);
        await _advertRepository.AddAdvert(advert, cancellationToken);

        // Save all changes
        await _unitOfWork.CompleteAsync(cancellationToken);

        // Map the response
        return new CreateAdvertResponse(
            advert.IdAdvert,
            advert.IdUser
        );
    }

    public async Task ChangeAdvertData(Guid idAdvert, ChangeAdvertDataRequest changeAdvertDataRequest,
        CancellationToken cancellationToken)
    {
        // Validate the idAdvert
        if (idAdvert == Guid.Empty)
        {
            throw new ArgumentException("IdAdvert cannot be empty.", nameof(idAdvert));
        }

        // Check if the advert exists
        Advert? advertByIdAdvert =
            await _advertRepository.GetActiveAndNonDeletedAdvertByIdAdvert(idAdvert, cancellationToken);

        if (advertByIdAdvert == null)
        {
            throw new Exception("Advert not found.");
        }

        // Check if the user is authorized to edit the advert (either the owner or an administrator)
        if (advertByIdAdvert.IdUser != _currentUserService.IdUser && !_currentUserService.IsAdministrator)
        {
            throw new UnauthorizedAccessException("Specified advert does not belong to you.");
        }

        // Perform the update
        advertByIdAdvert.PartialUpdate(
            changeAdvertDataRequest.Title,
            changeAdvertDataRequest.Description,
            changeAdvertDataRequest.PortfolioUrl,
            changeAdvertDataRequest.Price
        );

        // Save the changes
        await _unitOfWork.CompleteAsync(cancellationToken);
    }

    public async Task SoftDeleteAdvert(Guid idAdvert, CancellationToken cancellationToken)
    {
        // Validate the idAdvert
        if (idAdvert == Guid.Empty)
        {
            throw new ArgumentException("IdAdvert cannot be empty.", nameof(idAdvert));
        }

        // Check if the advert exists
        Advert? advertAndAdvertLog =
            await _advertRepository.GetActiveAndNonDeletedAdvertAndAdvertLogByIdAdvert(idAdvert, cancellationToken);

        if (advertAndAdvertLog == null)
        {
            throw new Exception("Advert with the provided idAdvert was not found.");
        }

        // Check if the user is authorized to delete the advert (either the owner or an administrator)
        if (advertAndAdvertLog.IdUser != _currentUserService.IdUser && !_currentUserService.IsAdministrator)
        {
            throw new UnauthorizedAccessException("Specified advert does not belong to you.");
        }

        // Mark the advert as deleted
        advertAndAdvertLog.AdvertLog.MarkAsDeleted();


        // Save the changes
        await _unitOfWork.CompleteAsync(cancellationToken);
    }

    public async Task<Guid> GetAdvertIdAdvertByIdUser(Guid idUser, CancellationToken cancellationToken)
    {
        if (idUser == Guid.Empty)
        {
            throw new ArgumentException("IdUser cannot be empty.", nameof(idUser));
        }

        Guid? idAdvertBasedOnIdUser =
            await _advertRepository.GetActiveAndNonDeletedIdAdvertByIdUser(idUser, cancellationToken);

        if (idAdvertBasedOnIdUser == null)
        {
            throw new Exception("Advert not found.");
        }

        return idAdvertBasedOnIdUser.Value;
    }

    public async Task<GetAdvertDetailsResponse> GetAdvertAssociatedDataByIdUser(Guid idUser,
        CancellationToken cancellationToken)
    {
        // Validate the advert ID and its existence
        if (idUser == Guid.Empty)
        {
            throw new ArgumentException("IdUser cannot be empty.", nameof(idUser));
        }

        AdvertDetailsDto? advert =
            await _advertRepository.GetActiveAndNonDeletedAdvertAssociatedDataByIdUser(idUser, cancellationToken);

        if (advert == null)
        {
            throw new Exception("You have not posted an advert yet.");
        }

        // Generate a presigned URL for the cover image
        string preSignedUrl = await _s3Service.TryGetPreSignedUrlAsync(advert.CoverImageKey, cancellationToken);

        // Map the response to GetAdvertResponse
        return new GetAdvertDetailsResponse(
            advert.IdUser,
            advert.IdAdvert,
            advert.Title!,
            advert.Description!,
            advert.Price,
            advert.CategoryName!,
            preSignedUrl,
            advert.PortfolioUrl!,
            advert.UserFirstName!,
            advert.UserLastName!,
            advert.DateCreated,
            advert.DateModified
        );
    }

    public async Task<GetAdvertDetailsResponse> GetAdvertAssociatedDataByIdAdvert(Guid idAdvert,
        CancellationToken cancellationToken)
    {
        // Validate the advert ID and its existence
        if (idAdvert == Guid.Empty)
        {
            throw new ArgumentException("IdAdvert cannot be empty.", nameof(idAdvert));
        }

        AdvertDetailsDto? advert =
            await _advertRepository.GetActiveAndNonDeletedAdvertAssociatedDataByIdAdvert(idAdvert, cancellationToken);

        if (advert == null)
        {
            throw new Exception("You have not posted an advert yet.");
        }

        // Generate a presigned URL for the cover image
        string preSignedUrl = await _s3Service.TryGetPreSignedUrlAsync(advert.CoverImageKey, cancellationToken);

        // Map the response to GetAdvertResponse
        return new GetAdvertDetailsResponse(
            advert.IdUser,
            advert.IdAdvert,
            advert.Title!,
            advert.Description!,
            advert.Price,
            advert.CategoryName!,
            preSignedUrl,
            advert.PortfolioUrl!,
            advert.UserFirstName!,
            advert.UserLastName!,
            advert.DateCreated,
            advert.DateModified
        );
    }

    public async Task<PagedListDto<AdvertOverviewDto>> GetAllAdvertsSummaries(string? sortOrder, int page, int pageSize,
        string? searchTerm,
        CancellationToken cancellationToken)
    {
        // Fetch paginated adverts
        PagedListDto<AdvertOverviewDto> allAdvertsWithPagination =
            await _advertRepository.GetAllActiveAndNonDeletedAdvertsSummariesWithPagination(sortOrder, page, pageSize,
                searchTerm,
                cancellationToken);


        // Generate presigned URLs for cover images
        foreach (AdvertOverviewDto advert in allAdvertsWithPagination.Items)
        {
            advert.CoverImageUrl =
                await _s3Service.TryGetPreSignedUrlAsync(advert.CoverImageKey, cancellationToken);
        }

        return allAdvertsWithPagination;
    }

    public async Task<Guid> MockImageUpload(IFormFile coverImageFile, CancellationToken cancellationToken)
    {
        Guid key = await _s3Service.TryUploadFileAsync(coverImageFile, cancellationToken);
        return key;
    }

    public async Task<Guid> AddReview(AddReviewRequest addReviewRequest,
        CancellationToken cancellationToken)
    {
        // Check if the user has already posted a review under the requested advert
        var findReviewForAdvertByIdUserAndIdAdvert = await _advertRepository.FindReviewForAdvertByIdUserAndIdAdvert(
            addReviewRequest.IdAdvert,
            _currentUserService.IdUser,
            cancellationToken
        );

        if (findReviewForAdvertByIdUserAndIdAdvert != Guid.Empty)
        {
            throw new Exception("You have already posted a review under this advert.");
        }

        // Create a ReviewLog
        ReviewLog reviewLog = ReviewLog.Create();

        // Create a Review
        Review review = Review.Create(addReviewRequest.IdAdvert,
            reviewLog.IdReviewLog,
            _currentUserService.IdUser,
            addReviewRequest.Content,
            addReviewRequest.SatisfactionLevel
        );

        // Persist the data
        await _advertRepository.AddReviewLog(reviewLog, cancellationToken);
        await _advertRepository.AddReview(review, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return review.IdReview;
    }

    public async Task<PagedListDto<ReviewDto>> GetReviewsForAdvertPaginated(Guid idAdvert, int page, int pageSize,
        CancellationToken cancellationToken)
    {
        if (idAdvert == Guid.Empty)
        {
            throw new ArgumentException("IdAdvert cannot be empty.", nameof(idAdvert));
        }

        PagedListDto<ReviewDto> reviewsForAdvertPaginated =
            await _advertRepository.GetReviewsForAdvertPaginated(idAdvert, page, pageSize, cancellationToken);

        return reviewsForAdvertPaginated;
    }
}