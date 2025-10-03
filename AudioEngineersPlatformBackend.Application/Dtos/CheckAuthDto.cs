namespace AudioEngineersPlatformBackend.Application.Dtos;

public class CheckAuthDto
{
    public required Guid IdUser { get; set; }
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string PhoneNumber { get; set; }
    public required Guid IdRole { get; set; }
    public required string RoleName { get; set; }
}