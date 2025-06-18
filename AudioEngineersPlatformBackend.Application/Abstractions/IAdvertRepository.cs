using AudioEngineersPlatformBackend.Application.Dtos;
using AudioEngineersPlatformBackend.Domain.Entities;

namespace AudioEngineersPlatformBackend.Application.Abstractions;

public interface IAdvertRepository
{
    Task<Advert?> GetActiveAndNonDeletedAdvertByIdUserAsync(Guid idUser, CancellationToken cancellationToken);
    Task<Advert?> GetActiveAndNonDeletedAdvertByIdAdvertAsync(Guid idAdvert, CancellationToken cancellationToken);

    Task<Advert?>
        GetActiveAndNonDeletedAdvertAndAdvertLogByIdAdvertAsync(Guid idAdvert, CancellationToken cancellationToken);

    Task<AdvertDetailsDto?> GetActiveAndNonDeletedAdvertAssociatedDataByIdUserAsync(Guid idUser,
        CancellationToken cancellationToken);

    Task<AdvertDetailsDto?> GetActiveAndNonDeletedAdvertAssociatedDataByIdAdvertAsync(Guid idAdvert,
        CancellationToken cancellationToken);

    Task<AdvertCategory?> GetAdvertCategoryByCategoryNameAsync(string categoryName, CancellationToken cancellationToken);
    Task AddAdvertAsync(Advert advert, CancellationToken cancellationToken);
    Task AddAdvertLogAsync(AdvertLog advertLog, CancellationToken cancellationToken);

    Task<PagedListDto<AdvertOverviewDto>> GetAllActiveAndNonDeletedAdvertsSummariesWithPaginationAsync(
        string? sortOrder,
        int page,
        int pageSize,
        string? searchTerm, CancellationToken cancellationToken
    );

    Task<Guid?> GetActiveAndNonDeletedIdAdvertByIdUserAsync(Guid idUser, CancellationToken cancellationToken);

    Task<Guid> FindReviewForAdvertByIdUserAndIdAdvertAsync(Guid idAdvert, Guid idUser,
        CancellationToken cancellationToken);

    Task AddReviewAsync(Review review, CancellationToken cancellationToken);

    Task AddReviewLogAsync(ReviewLog reviewLog, CancellationToken cancellationToken);

    Task<PagedListDto<ReviewDto>> GetReviewsForAdvertPaginatedAsync(Guid idAdvert, int page, int pageSize,
        CancellationToken cancellationToken);
}