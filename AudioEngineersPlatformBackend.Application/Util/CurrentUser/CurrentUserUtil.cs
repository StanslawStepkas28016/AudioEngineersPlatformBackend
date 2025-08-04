using System.Security.Claims;
using AudioEngineersPlatformBackend.Application.Abstractions;
using Microsoft.AspNetCore.Http;

namespace AudioEngineersPlatformBackend.Application.Util.CurrentUser;

public sealed class CurrentUserUtil : ICurrentUserUtil
{
    private readonly Guid _idUser;
    private readonly bool _isAdministrator;

    public Guid IdUser
    {
        get
        {
            EnsureAuthenticated();
            return _idUser;
        }
    }

    public bool IsAdministrator
    {
        get
        {
            EnsureAuthenticated();
            return _isAdministrator;
        }
    }

    /// <summary>
    ///     Method used for validating if 
    /// </summary>
    /// <exception cref="Exception"></exception>
    private void EnsureAuthenticated()
    {
        if (_idUser == Guid.Empty)
        {
            throw new Exception(
                $"You are trying to access the {nameof(CurrentUserUtil)} while not being authenticated."
            );
        }
    }

    /// <summary>
    ///     Constructor used for accessing the requests http context and extracting the users claims.
    /// </summary>
    /// <param name="httpContextAccessor"></param>
    public CurrentUserUtil(IHttpContextAccessor httpContextAccessor)
    {
        // Extract the idUser.
        var idUser
            = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        Guid.TryParse(idUser, out _idUser);

        // Extract the role claim boolean.
        _isAdministrator
            = httpContextAccessor.HttpContext.User.IsInRole("Administrator");
    }
}