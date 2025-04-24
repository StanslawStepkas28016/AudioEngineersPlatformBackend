using AudioEngineersPlatformBackend.Models;

namespace AudioEngineersPlatformBackend.Dtos.Auth.Utilities;

public class UserAndUserLogDto
{
    public User User { get; set; }
    public UserLog UserLog { get; set; }
}