namespace AudioEngineersPlatformBackend.Application.Dtos;

public class UserAssociatedDataDto
{
    public Guid IdUser { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public Guid IdRole { get; set; }
    public string? RoleName { get; set; }
};