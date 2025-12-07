using System.Security.Claims;
using API.Abstractions;

namespace API.Util.ClaimsUtil;

public class ClaimsUtil : IClaimsUtil
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ClaimsUtil(
        IHttpContextAccessor httpContextAccessor
    )
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Task<Guid> ExtractIdUserFromClaims()
    {
        return Task.FromResult
        (
            Guid.Parse
            (
                _httpContextAccessor
                    .HttpContext!
                    .User
                    .FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception("IdUser not found.")
            )
        );
    }

    public Task<string> ExtractRoleNameFromClaims()
    {
        return Task.FromResult
        (
            _httpContextAccessor
                .HttpContext!
                .User
                .FindFirstValue(ClaimTypes.Role) ?? throw new Exception("IdUser not found.")
        );
    }
}