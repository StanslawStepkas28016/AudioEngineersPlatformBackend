using AudioEngineersPlatformBackend.Application.CQRS.Chat.Queries.GetPresignedUrlForUpload;
using AutoMapper;

namespace API.Contracts.Chat.Commands.GetPreSignedUrlForUpload;

public class GetPreSignedUrlForUploadProfile : Profile
{
    public GetPreSignedUrlForUploadProfile()
    {
        CreateMap<GetPreSignedUrlForUploadRequest, GetPresignedUrlForUploadQuery>();
        CreateMap<GetPresignedUrlForUploadQueryResult, GetPresignedUrlForUploadResponse>();
    }
}