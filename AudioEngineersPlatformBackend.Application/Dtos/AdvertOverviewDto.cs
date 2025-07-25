namespace AudioEngineersPlatformBackend.Application.Dtos;

public class AdvertOverviewDto
{
    public Guid IdAdvert { get; set; }
    public string? Title { get; set; }
    public Guid IdUser { get; set; }
    public string? UserFirstName { get; set; }
    public string? UserLastName { get; set; }
    public DateTime DateCreated { get; set; }
    public double Price { get; set; }
    public string? CategoryName { get; set; }
    public Guid CoverImageKey { get; set; }
    public string? CoverImageUrl { get; set; }
    public string? Description { get; set; }
}