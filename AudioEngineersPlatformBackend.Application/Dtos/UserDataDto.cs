namespace AudioEngineersPlatformBackend.Application.Dtos;

public class UserDataDto
{
    public required Guid IdUser { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}