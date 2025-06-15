using System.IdentityModel.Tokens.Jwt;
using AudioEngineersPlatformBackend.Domain.Entities;

namespace AudioEngineersPlatformBackend.Application.Abstractions;

public interface ITokenUtil
{
    public JwtSecurityToken CreateJwtAccessToken(User user);
    public string CreateNonJwtRefreshToken();
}