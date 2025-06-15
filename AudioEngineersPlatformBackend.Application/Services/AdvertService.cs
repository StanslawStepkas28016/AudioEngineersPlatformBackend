using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Contracts.Advert;

namespace AudioEngineersPlatformBackend.Application.Services;

public class AdvertService : IAdvertService
{
    public async Task<CreateAdvertResponse> CreateAdvert(CreateAdvertRequest createAdvertRequest,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException("Not implemented yet");
    }
}