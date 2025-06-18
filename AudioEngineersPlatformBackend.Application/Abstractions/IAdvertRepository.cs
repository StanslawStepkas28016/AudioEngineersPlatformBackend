using AudioEngineersPlatformBackend.Application.Dtos;
using AudioEngineersPlatformBackend.Domain.Entities;

namespace AudioEngineersPlatformBackend.Application.Abstractions;

public interface IAdvertRepository
{
    Task<Advert?> GetAdvertByIdUser(Guid idUser, CancellationToken cancellationToken);
    Task<AdvertAssociatedData?> GetAdvertAssociatedDataByIdAdvert(Guid idAdvert, CancellationToken cancellationToken);
    Task<AdvertCategory?> GetAdvertCategoryByCategoryName(string categoryName, CancellationToken cancellationToken);
    Task<Advert> AddAdvert(Advert advert, CancellationToken cancellationToken);
    Task<AdvertLog> AddAdvertLog(AdvertLog advertLog, CancellationToken cancellationToken);
}