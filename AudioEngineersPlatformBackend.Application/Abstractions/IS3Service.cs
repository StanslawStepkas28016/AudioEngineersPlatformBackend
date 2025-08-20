using Microsoft.AspNetCore.Http;

namespace AudioEngineersPlatformBackend.Application.Abstractions;

public interface IS3Service
{
    public Task<Guid> UploadFileAsync(IFormFile file,
        CancellationToken cancellationToken);

    public Task<string> GetPreSignedUrlAsync(Guid key,
        CancellationToken cancellationToken);
}