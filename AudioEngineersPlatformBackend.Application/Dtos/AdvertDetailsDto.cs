namespace AudioEngineersPlatformBackend.Application.Dtos;

public class AdvertDetailsDto
{
    public Guid IdAdvert { get; set; }
    public Guid IdUser { get; set; }
    public string? UserFirstName { get; set; }
    public string? UserLastName { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public double Price { get; set; }
    public string? CategoryName { get; set; }
    public Guid CoverImageKey { get; set; }
    public string? PortfolioUrl { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateModified { get; set; }
}