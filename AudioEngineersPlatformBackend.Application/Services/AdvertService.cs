using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Application.Dtos;
using AudioEngineersPlatformBackend.Contracts.Advert;
using AudioEngineersPlatformBackend.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace AudioEngineersPlatformBackend.Application.Services;

public class AdvertService : IAdvertService
{
    private readonly IAdvertRepository _advertRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IS3Service _s3Service;

    public AdvertService(IAdvertRepository advertRepository, IUnitOfWork unitOfWork, IS3Service s3Service)
    {
        _advertRepository = advertRepository;
        _unitOfWork = unitOfWork;
        _s3Service = s3Service;
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
        var advert = new Advert(
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

        await _unitOfWork.CompleteAsync(cancellationToken);

        // Map the response
        return new CreateAdvertResponse(
            addedAdvert.IdAdvert,
            addedAdvert.IdUser
        );
    }

    public async Task<GetAdvertResponse> GetUserAdvert(Guid idUser, CancellationToken cancellationToken)
    {
        // Validate the advert ID and its existence
        if (idUser == Guid.Empty)
        {
            throw new ArgumentException("Advert ID cannot be empty.", nameof(idUser));
        }

        var advert = await _advertRepository.GetAdvertAssociatedDataByIdUser(idUser, cancellationToken);

        if (advert == null)
        {
            throw new Exception("You have not posted an advert yet.");
        }

        // Generate a presigned URL for the cover image
        var preSignedUrl = await _s3Service.TryGetPreSignedUrlAsync(advert.CoverImageKey, cancellationToken);

        // Map the response to GetAdvertResponse
        return new GetAdvertResponse(
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

    public async Task<PagedListDto<AdvertOverviewDto>> GetAllAdverts(string? sortOrder, int page, int pageSize,
        CancellationToken cancellationToken)
    {
        var allAdvertsWithPagination =
            await _advertRepository.GetAllAdvertsWithPagination(sortOrder, page, pageSize, cancellationToken);
        return allAdvertsWithPagination;
    }
}