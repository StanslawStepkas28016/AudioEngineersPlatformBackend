using MediatR;

namespace AudioEngineersPlatformBackend.Application.CQRS.Auth.Queries.CheckAuth;

public class CheckAuthQuery : IRequest<CheckAuthQueryResult>
{
    public required Guid IdUser { get; set; }
}