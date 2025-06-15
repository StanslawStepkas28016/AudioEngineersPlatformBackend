using AudioEngineersPlatformBackend.Contracts.Auth;

namespace AudioEngineersPlatformBackend.Application.Abstractions;

public interface IAuthService
{
    Task<RegisterResponse> Register(RegisterRequest registerRequest, CancellationToken cancellationToken = default);

    Task<VerifyAccountResponse> VerifyAccount(VerifyAccountRequest verifyAccountRequest,
        CancellationToken cancellationToken = default);

    Task<LoginResponse> Login(LoginRequest loginRequest, CancellationToken cancellationToken = default);

    Task Logout();

    Task<RefreshTokenResponse> RefreshToken(RefreshTokenRequest refreshTokenRequest,
        CancellationToken cancellationToken = default);

    Task<CheckAuthResponse> CheckAuth(Guid idUser, CancellationToken cancellationToken = default);
}