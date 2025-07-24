using AudioEngineersPlatformBackend.Application.Dtos;
using AudioEngineersPlatformBackend.Contracts.Advert;
using AudioEngineersPlatformBackend.Contracts.Advert.AddReview;
using AudioEngineersPlatformBackend.Contracts.Advert.ChangeAdverData;
using AudioEngineersPlatformBackend.Contracts.Advert.CreateAdvert;
using AudioEngineersPlatformBackend.Contracts.Advert.GetAdvertDetails;
using AudioEngineersPlatformBackend.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace AudioEngineersPlatformBackend.Application.Abstractions;

public interface IAdvertService
{
    Task<CreateAdvertResponse> CreateAdvert(CreateAdvertRequest createAdvertRequest,
        CancellationToken cancellationToken);

    Task ChangeAdvertData(Guid idAdvert, ChangeAdvertDataRequest changeAdvertDataRequest,
        CancellationToken cancellationToken);

    Task SoftDeleteAdvert(Guid idAdvert, CancellationToken cancellationToken);
    Task<Guid> GetAdvertIdAdvertByIdUser(Guid idUser, CancellationToken cancellationToken);

    Task<GetAdvertDetailsResponse> GetAdvertAssociatedDataByIdUser(Guid idUser,
        CancellationToken cancellationToken);

    Task<GetAdvertDetailsResponse> GetAdvertAssociatedDataByIdAdvert(Guid idAdvert,
        CancellationToken cancellationToken);

    Task<PagedListDto<AdvertOverviewDto>> GetAllAdvertsSummaries(string? sortOrder, int page, int pageSize,
        string? searchTerm,
        CancellationToken cancellationToken);

    Task<Guid> MockImageUpload(IFormFile coverImageFile, CancellationToken cancellationToken);

    Task<Guid> AddReview(AddReviewRequest addReviewRequest,
        CancellationToken cancellationToken);

    Task<PagedListDto<ReviewDto>> GetReviewsForAdvertPaginated(Guid idAdvert, int page, int pageSize,
        CancellationToken cancellationToken);
}