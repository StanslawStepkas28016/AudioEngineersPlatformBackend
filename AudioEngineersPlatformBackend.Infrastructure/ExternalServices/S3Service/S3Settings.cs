namespace AudioEngineersPlatformBackend.Infrastructure.ExternalServices.S3Service;

public class S3Settings
{
    public string AccessKey { get; init; } = string.Empty;
    public string SecretKey { get; init; } = string.Empty;
    public string Region { get; init; } = string.Empty;
    public string BucketName { get; init; } = string.Empty;
}