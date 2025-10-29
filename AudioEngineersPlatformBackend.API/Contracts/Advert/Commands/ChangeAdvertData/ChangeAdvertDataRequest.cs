namespace API.Contracts.Advert.Commands.ChangeAdvertData;

public class ChangeAdvertDataRequest
{
    public required Guid IdUser { get; set; }
    public required string Title { get; set; } 
    public required string Description { get; set; } 
    public required string PortfolioUrl { get; set; } 
    public required double Price { get; set; }
}