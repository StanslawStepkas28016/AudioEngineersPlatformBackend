using AudioEngineersPlatformBackend.Application.Dtos;
using AudioEngineersPlatformBackend.Contracts.Advert;
using Microsoft.AspNetCore.Http;

namespace AudioEngineersPlatformBackend.Application.Abstractions;

public interface IAdvertService
{
    Task<CreateAdvertResponse> CreateAdvert(CreateAdvertRequest createAdvertRequest,
        CancellationToken cancellationToken);

    Task<GetAdvertResponse> GetUserAdvert(Guid idUser,
        CancellationToken cancellationToken);

    Task<PagedListDto<AdvertOverviewDto>> GetAllAdverts(string? sortOrder, int page, int pageSize,
        CancellationToken cancellationToken);
}