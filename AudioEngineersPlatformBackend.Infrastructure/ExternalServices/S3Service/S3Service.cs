using System.Web;
using Amazon.S3;
using Amazon.S3.Model;
using AudioEngineersPlatformBackend.Application.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace AudioEngineersPlatformBackend.Infrastructure.ExternalServices.S3Service;

public class S3Service : IS3Service
{
    private readonly S3Settings _settings;
    private readonly IAmazonS3 _s3Client;

    public S3Service(IOptions<S3Settings> settings, IAmazonS3 s3Client)
    {
        _settings = settings.Value;
        _s3Client = s3Client;
    }

    public async Task<Guid> UploadFileAsync(string folder, IFormFile file,
        CancellationToken cancellationToken)
    {
        if (file.Length == 0)
        {
            throw new ArgumentException("File is empty.");
        }

        await using Stream? stream = file.OpenReadStream();

        Guid key = Guid.NewGuid();

        PutObjectRequest request = new PutObjectRequest
        {
            BucketName = _settings.BucketName,
            Key = $"{folder}/{key}",
            InputStream = stream,
            ContentType = file.ContentType,
            Metadata =
            {
                ["file-name"] = file.FileName,
            }
        };

        // This can throw an exception if the upload fails.
        await _s3Client.PutObjectAsync(request, cancellationToken);

        return key;
    }

    public async Task<string> GetPreSignedUrlForReadAsync(string folder, string fileName, Guid key,
        CancellationToken cancellationToken)
    {
        if (key == Guid.Empty)
        {
            throw new ArgumentException($"{nameof(key)} is empty.");
        }

        GetPreSignedUrlRequest request = new GetPreSignedUrlRequest
        {
            BucketName = _settings.BucketName,
            Key = $"{folder}/{key}",
            Expires = DateTime.UtcNow.AddMinutes(15),
            Verb = HttpVerb.GET,
        };

        // Provide a UTF-8 encoded content disposition for appropriate file name when downloading a file.
        request.ResponseHeaderOverrides.ContentDisposition =
            $"attachment;" +
            $"filename*=UTF-8''{HttpUtility.UrlEncode(fileName)}";

        // This can throw an exception if the URL generation fails.
        string? presignedUrl = await _s3Client.GetPreSignedURLAsync(request);

        return presignedUrl;
    }

    public async Task<string> GetPreSignedUrlForUploadAsync(string folder, Guid key, string fileName,
        CancellationToken cancellationToken)
    {
        if (key == Guid.Empty)
        {
            throw new ArgumentException($"{nameof(key)} is empty.");
        }

        GetPreSignedUrlRequest request = new GetPreSignedUrlRequest
        {
            BucketName = _settings.BucketName,
            Key = $"{folder}/{key}",
            Expires = DateTime.UtcNow.AddMinutes(2),
            Verb = HttpVerb.PUT,
            Metadata =
            {
                ["file-name"] = fileName,
            }
        };

        // This can throw an exception if the URL generation fails.
        string? presignedUrl = await _s3Client.GetPreSignedURLAsync(request);

        return presignedUrl;
    }
}