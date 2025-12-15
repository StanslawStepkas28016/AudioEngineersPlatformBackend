using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Abstractions;
using API.Config.Settings;
using AudioEngineersPlatformBackend.Application.Config.Settings;
using AudioEngineersPlatformBackend.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace API.Util.JwtUtil;

public class JwtUtil : IJwtUtil
{
    private readonly JwtSettings _jwtSettings;

    public JwtUtil(
        IOptions<JwtSettings> jwtSettings
    )
    {
        _jwtSettings = jwtSettings.Value;
    }

    public Task<JwtSecurityToken> CreateJwtAccessToken(
        User user
    )
    {
        // Claims setup.
        List<Claim> claims =
        [
            new(JwtRegisteredClaimNames.Iss, _jwtSettings.Issuer),
            new(JwtRegisteredClaimNames.Sub, user.IdUser.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(ClaimTypes.Role, user.Role.RoleName)
        ];

        // Add multiple audiences.
        claims.AddRange(_jwtSettings.Audiences.Select(audience => new Claim(JwtRegisteredClaimNames.Aud, audience)));

        // Add symmetric creds.
        SymmetricSecurityKey secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        SigningCredentials credentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);

        // Create the token.
        JwtSecurityToken token = new JwtSecurityToken
        (
            claims: claims,
            expires: DateTime.UtcNow.AddHours(_jwtSettings.ExpireHours),
            signingCredentials: credentials
        );

        return Task.FromResult(token);
    }
}