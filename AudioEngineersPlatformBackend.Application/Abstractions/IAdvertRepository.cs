using AudioEngineersPlatformBackend.Application.Dtos;
using AudioEngineersPlatformBackend.Domain.Entities;

namespace AudioEngineersPlatformBackend.Application.Abstractions;

public interface IAdvertRepository
{
    Task<Advert?> GetAdvertByIdUser(Guid idUser, CancellationToken cancellationToken);
    Task<AdvertDetailsDto?> GetAdvertAssociatedDataByIdUser(Guid idUser, CancellationToken cancellationToken);
    Task<AdvertCategory?> GetAdvertCategoryByCategoryName(string categoryName, CancellationToken cancellationToken);
    Task<Advert> AddAdvert(Advert advert, CancellationToken cancellationToken);
    Task<AdvertLog> AddAdvertLog(AdvertLog advertLog, CancellationToken cancellationToken);

    Task<PagedListDto<AdvertOverviewDto>> GetAllAdvertsWithPagination(string? sortOrder, int page, int pageSize,
        CancellationToken cancellationToken);
}