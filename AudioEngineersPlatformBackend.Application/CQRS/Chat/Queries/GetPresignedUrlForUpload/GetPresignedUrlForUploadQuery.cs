using MediatR;

namespace AudioEngineersPlatformBackend.Application.CQRS.Chat.Queries.GetPresignedUrlForUpload;

public class GetPresignedUrlForUploadQuery : IRequest<GetPresignedUrlForUploadQueryResult>
{
    public required string Folder { get; set; } 
    public required string FileName { get; set; } 
}