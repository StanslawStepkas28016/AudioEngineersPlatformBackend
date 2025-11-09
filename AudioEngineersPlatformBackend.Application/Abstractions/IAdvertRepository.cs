using AudioEngineersPlatformBackend.Application.Dtos;
using AudioEngineersPlatformBackend.Domain.Entities;

namespace AudioEngineersPlatformBackend.Application.Abstractions;

public interface IAdvertRepository
{
    Task<bool> DoesUserHaveAnyActiveAdvertByIdUserAsync(
        Guid idUser,
        CancellationToken cancellationToken
    );

    Task<AdvertCategory?> FindAdvertCategoryByNameAsync(
        string categoryName,
        CancellationToken cancellationToken
    );

    Task AddAdvertAsync(
        Advert advert,
        CancellationToken cancellationToken
    );

    Task AddAdvertLogAsync(
        AdvertLog advertLog,
        CancellationToken cancellationToken
    );

    Task<Advert?> FindAdvertAndAdvertLogByIdUserAndIdAdvertAsync(
        Guid idUser,
        Guid idAdvert,
        CancellationToken cancellationToken
    );

    Task<bool> DoesUserHaveAReviewPostedAlreadyByIdUserReviewerAndIdAdvert(
        Guid idUserReviewer,
        Guid idAdvert,
        CancellationToken cancellationToken
    );

    Task<bool> DoesAdvertExistByIdAdvertAsync(
        Guid idAdvert,
        CancellationToken cancellationToken
    );

    Task AddReviewLogAsync(
        ReviewLog reviewLog,
        CancellationToken cancellationToken
    );

    Task AddReviewAsync(
        Review review,
        CancellationToken cancellationToken
    );

    Task<AdvertDetailsDto?> FindAdvertDetailsByIdAdvertAsync(
        Guid idAdvert,
        CancellationToken cancellationToken
    );

    Task<PagedListDto<AdvertSummaryDto>> FindAdvertSummariesAsync(
        string? sortOrder,
        int page,
        int pageSize,
        string? searchTerm,
        CancellationToken cancellationToken
    );

    Task<PagedListDto<ReviewDto>> FindAdvertReviewsAsync(
        Guid idAdvert,
        int page,
        int pageSize,
        CancellationToken cancellationToken
    );

    Task<Guid> FindIdAdvertByIdUser(
        Guid idUser,
        CancellationToken cancellationToken
    );
}