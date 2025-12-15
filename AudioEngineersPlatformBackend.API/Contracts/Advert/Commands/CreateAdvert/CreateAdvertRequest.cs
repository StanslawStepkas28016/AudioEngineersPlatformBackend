namespace API.Contracts.Advert.Commands.CreateAdvert;

public class CreateAdvertRequest
{
    public required Guid IdUser { get; set; }
    public required string Title { get; set; } 
    public required string Description { get; set; } 
    public required IFormFile CoverImageFile { get; set; }
    public required string PortfolioUrl { get; set; } 
    public required double Price { get; set; }
    public required string CategoryName { get; set; } 
}