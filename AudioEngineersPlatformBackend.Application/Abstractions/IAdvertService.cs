using AudioEngineersPlatformBackend.Application.Dtos;
using AudioEngineersPlatformBackend.Contracts.Advert;
using AudioEngineersPlatformBackend.Contracts.Advert.ChangeAdverData;
using AudioEngineersPlatformBackend.Contracts.Advert.CreateAdvert;
using AudioEngineersPlatformBackend.Contracts.Advert.GetAdvertDetails;
using Microsoft.AspNetCore.Http;

namespace AudioEngineersPlatformBackend.Application.Abstractions;

public interface IAdvertService
{
    Task<CreateAdvertResponse> CreateAdvert(CreateAdvertRequest createAdvertRequest,
        CancellationToken cancellationToken);

    Task ChangeAdvertData(Guid idAdvert, ChangeAdvertDataRequest changeAdvertDataRequest,
        CancellationToken cancellationToken);

    Task DeleteAdvert(Guid idAdvert, CancellationToken cancellationToken);

    Task<GetAdvertDetailsResponse> GetAdvertAssociatedDataByIdUser(Guid idUser,
        CancellationToken cancellationToken);

    Task<GetAdvertDetailsResponse> GetAdvertAssociatedDataByIdAdvert(Guid idAdvert,
        CancellationToken cancellationToken);
    
    Task<PagedListDto<AdvertOverviewDto>> GetAllAdvertsSummaries(string? sortOrder, int page, int pageSize, string? searchTerm,
        CancellationToken cancellationToken);

    Task<Guid> MockImageUpload(IFormFile coverImageFile, CancellationToken cancellationToken);
}