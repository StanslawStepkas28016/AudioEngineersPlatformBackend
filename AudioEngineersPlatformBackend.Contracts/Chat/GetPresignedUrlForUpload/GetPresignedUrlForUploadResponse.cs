namespace AudioEngineersPlatformBackend.Contracts.Chat.GetPresignedUrlForUpload;

public sealed class GetPresignedUrlForUploadResponse
{
    public string PreSignedUrlForUpload { get; set; } = String.Empty;
    public Guid FileKey { get; set; }
}