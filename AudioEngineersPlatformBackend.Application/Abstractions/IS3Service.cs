using Amazon.Util.Internal;
using Microsoft.AspNetCore.Http;

namespace AudioEngineersPlatformBackend.Application.Abstractions;

public interface IS3Service
{
    public Task<Guid> TryUploadFileAsync(IFormFile file,
        CancellationToken cancellationToken);

    public Task<string> TryGetPreSignedUrlAsync(Guid key,
        CancellationToken cancellationToken = default);
}