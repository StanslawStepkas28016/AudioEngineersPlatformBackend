namespace AudioEngineersPlatformBackend.Application.CQRS.Chat.Queries.GetPresignedUrlForUpload;

public class GetPresignedUrlForUploadQueryResult
{
    public required Guid FileKey { get; set; }
    public required string PreSignedUrlForUpload { get; set; } 
}