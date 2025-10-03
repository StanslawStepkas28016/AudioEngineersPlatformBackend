namespace AudioEngineersPlatformBackend.Infrastructure.Config.Settings;

public class S3Settings
{
    public string AccessKey { get; init; } 
    public string SecretKey { get; init; } 
    public string Region { get; init; } 
    public string BucketName { get; init; } 
}