using AudioEngineersPlatformBackend.Application.Dtos;
using AudioEngineersPlatformBackend.Contracts.Advert;
using AudioEngineersPlatformBackend.Contracts.Advert.Create;
using AudioEngineersPlatformBackend.Contracts.Advert.Edit;
using AudioEngineersPlatformBackend.Contracts.Advert.Get;
using Microsoft.AspNetCore.Http;

namespace AudioEngineersPlatformBackend.Application.Abstractions;

public interface IAdvertService
{
    Task<CreateAdvertResponse> CreateAdvert(CreateAdvertRequest createAdvertRequest,
        CancellationToken cancellationToken);

    Task EditAdvert(Guid idAdvert, EditAdvertRequest editAdvertRequest,
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