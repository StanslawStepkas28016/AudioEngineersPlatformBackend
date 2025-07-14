using AudioEngineersPlatformBackend.Application.Dtos;
using AudioEngineersPlatformBackend.Domain.Entities;

namespace AudioEngineersPlatformBackend.Application.Abstractions;

public interface IAdvertRepository
{
    Task<Advert?> GetActiveAndNonDeletedAdvertByIdUser(Guid idUser, CancellationToken cancellationToken);
    Task<Advert?> GetActiveAndNonDeletedAdvertByIdAdvert(Guid idAdvert, CancellationToken cancellationToken);

    Task<Advert?>
        GetActiveAndNonDeletedAdvertAndAdvertLogByIdAdvert(Guid idAdvert, CancellationToken cancellationToken);

    Task<AdvertDetailsDto?> GetActiveAndNonDeletedAdvertAssociatedDataByIdUser(Guid idUser,
        CancellationToken cancellationToken);

    Task<AdvertDetailsDto?> GetActiveAndNonDeletedAdvertAssociatedDataByIdAdvert(Guid idAdvert,
        CancellationToken cancellationToken);

    Task<AdvertCategory?> GetAdvertCategoryByCategoryName(string categoryName, CancellationToken cancellationToken);
    Task AddAdvert(Advert advert, CancellationToken cancellationToken);
    Task AddAdvertLog(AdvertLog advertLog, CancellationToken cancellationToken);

    Task<PagedListDto<AdvertOverviewDto>> GetAllActiveAndNonDeletedAdvertsSummariesWithPagination(
        string? sortOrder,
        int page,
        int pageSize,
        string? searchTerm, CancellationToken cancellationToken
    );

    Task<Guid?> GetActiveAndNonDeletedIdAdvertByIdUser(Guid idUser, CancellationToken cancellationToken);

    Task<Guid> FindReviewForAdvertByIdUserAndIdAdvert(Guid idAdvert, Guid idUser,
        CancellationToken cancellationToken);

    Task AddReview(Review review, CancellationToken cancellationToken);

    Task AddReviewLog(ReviewLog reviewLog, CancellationToken cancellationToken);

    Task<PagedListDto<ReviewDto>> GetReviewsForAdvertPaginated(Guid idAdvert, int page, int pageSize,
        CancellationToken cancellationToken);
}