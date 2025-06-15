using AudioEngineersPlatformBackend.Application.Abstractions;

namespace AudioEngineersPlatformBackend.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

}