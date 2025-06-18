namespace AudioEngineersPlatformBackend.Infrastructure.ExternalServices.SESService;

public class SESSettings
{
    public string AccessKey { get; init; } = string.Empty;
    public string SecretKey { get; init; } = string.Empty;
    public string Region { get; init; } = string.Empty;
    public string SenderEmail { get; init; } = string.Empty;
}