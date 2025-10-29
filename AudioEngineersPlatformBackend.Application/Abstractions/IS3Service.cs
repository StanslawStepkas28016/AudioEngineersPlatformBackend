using Microsoft.AspNetCore.Http;

namespace AudioEngineersPlatformBackend.Application.Abstractions;

public interface IS3Service
{
    Task<Guid> UploadFileAsync(
        string folder,
        IFormFile file,
        CancellationToken cancellationToken
    );

    Task<string> GetPreSignedUrlForReadAsync(
        string folder,
        string fileName,
        Guid key,
        CancellationToken cancellationToken
    );

    Task<string> GetPreSignedUrlForUploadAsync(
        string folder,
        Guid key,
        string fileName,
        CancellationToken cancellationToken
    );
}