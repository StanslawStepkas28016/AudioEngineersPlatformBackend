using System.Threading;
using System.Threading.Tasks;
using AudioEngineersPlatformBackend.Dtos.Auth.Login;
using AudioEngineersPlatformBackend.Dtos.Auth.Register;
using AudioEngineersPlatformBackend.Dtos.Auth.VerifyEmail;

namespace AudioEngineersPlatformBackend.Services.AuthService;

public interface IAuthService
{
    public Task<RegisterResponseDto> Register(RegisterRequestDto registerRequestDto,
        CancellationToken cancellationToken);

    public Task<VerifyEmailResponseDto> VerifyEmail(VerifyEmailRequestDto verifyEmailRequestDto, CancellationToken cancellationToken); 


    public Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto, CancellationToken cancellationToken);
}