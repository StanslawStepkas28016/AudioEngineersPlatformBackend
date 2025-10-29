using MediatR;

namespace AudioEngineersPlatformBackend.Application.CQRS.Advert.Commands.ChangeAdvertData;

public class ChangeAdvertDataCommand : IRequest<ChangeAdvertDataCommandResult>
{
    public required Guid IdUser { get; set; }
    public required Guid IdAdvert { get; set; }
    public required string Title { get; set; } 
    public required string Description { get; set; } 
    public required string PortfolioUrl { get; set; } 
    public required double Price { get; set; }
}