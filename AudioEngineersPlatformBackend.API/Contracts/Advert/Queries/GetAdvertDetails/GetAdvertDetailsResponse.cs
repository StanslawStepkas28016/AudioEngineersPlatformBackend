namespace API.Contracts.Advert.Queries.GetAdvertDetails;

public class GetAdvertDetailsResponse
{
    public required Guid IdAdvert { get; set; }
    public required Guid IdUser { get; set; }
    public required string UserFirstName { get; set; } 
    public required string UserLastName { get; set; } 
    public required string Title { get; set; } 
    public required string Description { get; set; } 
    public required double Price { get; set; }
    public required string CategoryName { get; set; } 
    public required Guid CoverImageKey { get; set; }
    public required string CoverImageUrl { get; set; } 
    public required string PortfolioUrl { get; set; } 
    public required DateTime DateCreated { get; set; }
    public required DateTime? DateModified { get; set; }
}