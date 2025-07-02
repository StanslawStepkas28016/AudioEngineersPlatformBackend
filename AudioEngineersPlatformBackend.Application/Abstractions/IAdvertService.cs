using AudioEngineersPlatformBackend.Application.Dtos;
using AudioEngineersPlatformBackend.Contracts.Advert;
using Microsoft.AspNetCore.Http;

namespace AudioEngineersPlatformBackend.Application.Abstractions;

public interface IAdvertService
{
    Task<CreateAdvertResponse> CreateAdvert(CreateAdvertRequest createAdvertRequest,
        CancellationToken cancellationToken);

    Task<GetAdvertDetailsResponse> GetUserAdvert(Guid idUser,
        CancellationToken cancellationToken);

    Task<PagedListDto<AdvertOverviewDto>> GetAllAdverts(string? sortOrder, int page, int pageSize, string? searchTerm,
        CancellationToken cancellationToken);

    Task<Guid> MockImageUpload(IFormFile coverImageFile, CancellationToken cancellationToken);
}