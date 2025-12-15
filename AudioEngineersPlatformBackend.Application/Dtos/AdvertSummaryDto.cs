namespace AudioEngineersPlatformBackend.Application.Dtos;

public class AdvertSummaryDto
{
    public required Guid IdAdvert { get; set; }
    public required string Title { get; set; }
    public required Guid IdUser { get; set; }
    public required string UserFirstName { get; set; }
    public required string UserLastName { get; set; }
    public required DateTime DateCreated { get; set; }
    public required double Price { get; set; }
    public required string DescriptionShort { get; set; }
    public required string CategoryName { get; set; }
    public required Guid CoverImageKey { get; set; }
    public string? CoverImageUrl { get; set; } // Will be generated based on the CoverImageKey property.
    public required string Description { get; set; }
}