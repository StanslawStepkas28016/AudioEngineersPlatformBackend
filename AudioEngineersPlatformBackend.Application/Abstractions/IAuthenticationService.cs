using AudioEngineersPlatformBackend.Contracts.Authentication;

namespace AudioEngineersPlatformBackend.Application.Abstractions;

public interface IAuthenticationService
{
    Task<RegisterResponse> Register(RegisterRequest registerRequest, CancellationToken cancellationToken = default);
}