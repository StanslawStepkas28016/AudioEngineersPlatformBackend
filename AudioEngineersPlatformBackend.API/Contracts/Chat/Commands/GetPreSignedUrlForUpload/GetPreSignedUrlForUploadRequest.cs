namespace API.Contracts.Chat.Commands.GetPreSignedUrlForUpload;

public class GetPreSignedUrlForUploadRequest
{
    public required string Folder { get; set; } 
    public required string FileName { get; set; } 
}