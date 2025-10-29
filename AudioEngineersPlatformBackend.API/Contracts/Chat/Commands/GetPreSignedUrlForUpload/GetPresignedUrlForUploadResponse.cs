namespace API.Contracts.Chat.Commands.GetPreSignedUrlForUpload;

public sealed class GetPresignedUrlForUploadResponse
{
    public required Guid FileKey { get; set; }
    public required string PreSignedUrlForUpload { get; set; } 
}