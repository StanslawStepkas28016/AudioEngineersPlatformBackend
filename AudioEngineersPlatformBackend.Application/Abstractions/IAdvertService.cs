using AudioEngineersPlatformBackend.Contracts.Advert;
using Microsoft.AspNetCore.Http;

namespace AudioEngineersPlatformBackend.Application.Abstractions;

public interface IAdvertService
{
    Task<CreateAdvertResponse> CreateAdvert(CreateAdvertRequest createAdvertRequest,
        CancellationToken cancellationToken);
    
    Task<GetAdvertResponse> GetAdvert(Guid idAdvert,
        CancellationToken cancellationToken);
}