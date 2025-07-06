using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Application.Dtos;
using AudioEngineersPlatformBackend.Contracts.Advert;
using AudioEngineersPlatformBackend.Contracts.Advert.Create;
using AudioEngineersPlatformBackend.Contracts.Advert.Edit;
using AudioEngineersPlatformBackend.Contracts.Advert.Get;
using AudioEngineersPlatformBackend.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace AudioEngineersPlatformBackend.Application.Services;

public class AdvertService : IAdvertService
{
    private readonly IAdvertRepository _advertRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IS3Service _s3Service;
    private readonly ICurrentUserService _currentUserService;

    public AdvertService(IAdvertRepository advertRepository, IUnitOfWork unitOfWork, IS3Service s3Service,
        ICurrentUserService currentUserService)
    {
        _advertRepository = advertRepository;
        _unitOfWork = unitOfWork;
        _s3Service = s3Service;
        _currentUserService = currentUserService;
    }

    public async Task<CreateAdvertResponse> CreateAdvert(CreateAdvertRequest createAdvertRequest,
        CancellationToken cancellationToken)
    {
        // Validate the user from the request
        if (createAdvertRequest.IdUser == Guid.Empty)
        {
            throw new ArgumentException("User ID cannot be empty.");
        }

        // Check if there is already an advert posted by the user
        var advertByIdUser = await _advertRepository.GetAdvertByIdUser(createAdvertRequest.IdUser, cancellationToken);

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

        var advertCategory = await _advertRepository
            .GetAdvertCategoryByCategoryName(createAdvertRequest.CategoryName, cancellationToken);

        if (advertCategory == null)
        {
            throw new Exception("Category not found.");
        }

        // Create a new AdvertLog entity
        var advertLog = AdvertLog.Create();

        // Upload the cover image file to S3 and get the key
        var imageKey = await _s3Service.TryUploadFileAsync(createAdvertRequest.CoverImageFile, cancellationToken);

        // Create a new Advert entity
        var advert = Advert.Create(
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
        var addedAdvert = await _advertRepository.AddAdvert(advert, cancellationToken);

        // Save all changes
        await _unitOfWork.CompleteAsync(cancellationToken);

        // Map the response
        return new CreateAdvertResponse(
            addedAdvert.IdAdvert,
            addedAdvert.IdUser
        );
    }

    public async Task EditAdvert(Guid idAdvert, EditAdvertRequest editAdvertRequest,
        CancellationToken cancellationToken)
    {
        // Validate the idAdvert
        if (idAdvert == Guid.Empty)
        {
            throw new ArgumentException("IdAdvert cannot be empty.", nameof(idAdvert));
        }

        // Check if the advert exists
        var advertByIdAdvert = await _advertRepository.GetAdvertByIdAdvert(idAdvert, cancellationToken);

        if (advertByIdAdvert == null)
        {
            throw new Exception("Advert not found.");
        }

        // Check if the user is authorized to edit the advert (either the owner or an administrator)
        if (advertByIdAdvert.IdUser != _currentUserService.IdUser || !_currentUserService.IsAdministrator)
        {
            throw new UnauthorizedAccessException("Specified advert does not belong to you.");
        }

        // Perform the update
        advertByIdAdvert.PartialUpdate(
            editAdvertRequest.Title,
            editAdvertRequest.Description,
            editAdvertRequest.PortfolioUrl,
            editAdvertRequest.Price
        );

        // Save the changes
        await _unitOfWork.CompleteAsync(cancellationToken);
    }

    public async Task DeleteAdvert(Guid idAdvert, CancellationToken cancellationToken)
    {
        // Validate the idAdvert
        if (idAdvert == Guid.Empty)
        {
            throw new ArgumentException("IdAdvert cannot be empty.", nameof(idAdvert));
        }

        // Check if the advert exists
        var advertAndAdvertLog = await _advertRepository.GetAdvertAndAdvertLogByIdAdvert(idAdvert, cancellationToken);

        if (advertAndAdvertLog == null)
        {
            throw new Exception("Advert with the provided idAdvert was not found.");
        }

        // Check if the user is authorized to delete the advert (either the owner or an administrator)
        if (advertAndAdvertLog.IdUser != _currentUserService.IdUser || !_currentUserService.IsAdministrator)
        {
            throw new UnauthorizedAccessException("Specified advert does not belong to you.");
        }

        // Mark the advert as deleted
        advertAndAdvertLog.AdvertLog.MarkAsDeleted();

        // Save the changes
        await _unitOfWork.CompleteAsync(cancellationToken);
    }

    public async Task<GetAdvertDetailsResponse> GetAdvertAssociatedDataByIdUser(Guid idUser,
        CancellationToken cancellationToken)
    {
        // Validate the advert ID and its existence
        if (idUser == Guid.Empty)
        {
            throw new ArgumentException("IdUser cannot be empty.", nameof(idUser));
        }

        var advert = await _advertRepository.GetAdvertAssociatedDataByIdUser(idUser, cancellationToken);

        if (advert == null)
        {
            throw new Exception("You have not posted an advert yet.");
        }

        // Generate a presigned URL for the cover image
        var preSignedUrl = await _s3Service.TryGetPreSignedUrlAsync(advert.CoverImageKey, cancellationToken);

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

        var advert = await _advertRepository.GetAdvertAssociatedDataByIdUser(idAdvert, cancellationToken);

        if (advert == null)
        {
            throw new Exception("You have not posted an advert yet.");
        }

        // Generate a presigned URL for the cover image
        var preSignedUrl = await _s3Service.TryGetPreSignedUrlAsync(advert.CoverImageKey, cancellationToken);

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

        throw new NotImplementedException();
    }

    public async Task<PagedListDto<AdvertOverviewDto>> GetAllAdvertsSummaries(string? sortOrder, int page, int pageSize,
        string? searchTerm,
        CancellationToken cancellationToken)
    {
        // Fetch paginated adverts
        var allAdvertsWithPagination =
            await _advertRepository.GetAllAdvertsSummariesWithPagination(sortOrder, page, pageSize, searchTerm,
                cancellationToken);


        // Generate presigned URLs for cover images
        foreach (var advert in allAdvertsWithPagination.Items)
        {
            advert.CoverImageUrl =
                await _s3Service.TryGetPreSignedUrlAsync(advert.CoverImageKey, cancellationToken);
        }

        return allAdvertsWithPagination;
    }

    public async Task<Guid> MockImageUpload(IFormFile coverImageFile, CancellationToken cancellationToken)
    {
        var key = await _s3Service.TryUploadFileAsync(coverImageFile, cancellationToken);
        return key;
    }
}