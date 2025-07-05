using System.Security.Claims;
using AudioEngineersPlatformBackend.Application.Abstractions;
using Microsoft.AspNetCore.Http;

namespace AudioEngineersPlatformBackend.Application.Services;

public class CurrentUserService : ICurrentUserService
{
    public Guid IdUser { get; }
    public bool IsAdministrator { get; }

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        IdUser = Guid.Parse(
            httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        IsAdministrator
            = httpContextAccessor.HttpContext.User.IsInRole("Administrator");
    }
}