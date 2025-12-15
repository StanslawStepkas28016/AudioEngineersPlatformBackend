using MediatR;

namespace AudioEngineersPlatformBackend.Application.CQRS.Advert.Queries.GetIdAdvertByIdUser;

public class GetIdAdvertByIdUserQuery : IRequest<GetIdAdvertByIdUserQueryResult>
{
    public required Guid IdUser { get; set; }
}