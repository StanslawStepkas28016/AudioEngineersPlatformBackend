using System.IdentityModel.Tokens.Jwt;
using AudioEngineersPlatformBackend.Domain.Entities;

namespace API.Abstractions;

public interface IJwtUtil
{
    Task<JwtSecurityToken> CreateJwtAccessToken(
        User user
    );
}