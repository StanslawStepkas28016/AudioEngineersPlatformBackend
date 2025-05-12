using AudioEngineersPlatformBackend.Contracts.Authentication;

namespace AudioEngineersPlatformBackend.Application.Abstractions;

public interface IAuthenticationService
{
    Task<RegisterResponse> Register(RegisterRequest registerRequest, CancellationToken cancellationToken = default);
    Task<VerifyAccountResponse> VerifyAccount(VerifyAccountRequest verifyAccountRequest, CancellationToken cancellationToken = default);
    Task<LoginResponse> Login(LoginRequest loginRequest, CancellationToken cancellationToken = default);
}