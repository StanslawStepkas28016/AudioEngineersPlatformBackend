using MediatR;

namespace AudioEngineersPlatformBackend.Application.CQRS.Advert.Commands.DeleteAdvert;

public class DeleteAdvertCommand : IRequest<DeleteAdvertCommandResult>
{
    public required Guid IdAdvert { get; set; }
    public required Guid IdUser { get; set; }
}