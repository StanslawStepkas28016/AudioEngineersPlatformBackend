using MediatR;

namespace AudioEngineersPlatformBackend.Application.CQRS.Advert.Queries.GetAdvertDetails;

public class GetAdvertDetailsQuery : IRequest<GetAdvertDetailsQueryResult>
{
    public required Guid IdAdvert { get; set; }
}