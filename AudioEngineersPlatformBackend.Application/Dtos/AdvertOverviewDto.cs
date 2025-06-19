namespace AudioEngineersPlatformBackend.Application.Dtos;

public class AdvertOverviewDto
{
    public Guid IdAvert { get; set; }
    public string? UserFirstName { get; set; }
    public string? UserLastName { get; set; }
    public DateTime DateCreated { get; set; }
    public double Price { get; set; }
    public string? CategoryName { get; set; }
}