using System.Runtime.CompilerServices;
using System.Security.Claims;
using AudioEngineersPlatformBackend.Application.Abstractions;
using Microsoft.AspNetCore.Http;

namespace AudioEngineersPlatformBackend.Application.Services;

public sealed class CurrentUserService : ICurrentUserService
{
    private readonly Guid _idUser;
    private readonly bool _isAdministrator;

    public Guid IdUser
    {
        get
        {
            TryAuthenticate();
            return _idUser;
        }
    }

    public bool IsAdministrator
    {
        get
        {
            TryAuthenticate();
            return _isAdministrator;
        }
    }

    private void TryAuthenticate()
    {
        if (_idUser == Guid.Empty)
        {
            throw new Exception(
                $"You are trying to access the {nameof(CurrentUserService)} while not being authenticated!"
            );
        }
    }

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        var idUser
            = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        Guid.TryParse(idUser, out _idUser);

        _isAdministrator
            = httpContextAccessor.HttpContext.User.IsInRole("Administrator");
    }
}