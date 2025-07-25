using AudioEngineersPlatformBackend.Contracts.Auth;
using AudioEngineersPlatformBackend.Contracts.Auth.CheckAuth;
using AudioEngineersPlatformBackend.Contracts.Auth.Login;
using AudioEngineersPlatformBackend.Contracts.Auth.Register;
using AudioEngineersPlatformBackend.Contracts.Auth.VerifyAccount;

namespace AudioEngineersPlatformBackend.Application.Abstractions;

public interface IAuthService
{
    Task<RegisterResponse> Register(RegisterRequest registerRequest, CancellationToken cancellationToken = default);

    Task<VerifyAccountResponse> VerifyAccount(VerifyAccountRequest verifyAccountRequest,
        CancellationToken cancellationToken = default);

    Task<LoginResponse> Login(LoginRequest loginRequest, CancellationToken cancellationToken = default);

    Task Logout();

    Task RefreshToken(CancellationToken cancellationToken = default);

    Task<CheckAuthResponse> CheckAuth(Guid idUser, CancellationToken cancellationToken = default);
}