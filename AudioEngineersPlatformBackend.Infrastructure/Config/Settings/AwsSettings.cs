namespace AudioEngineersPlatformBackend.Infrastructure.Config.Settings;

public class AwsSettings
{
    public required string AccessKey { get; init; }
    public required string SecretKey { get; init; }
    public required string Region { get; init; }
    public required string SenderEmail { get; init; }
    public required string BucketName { get; init; }
}