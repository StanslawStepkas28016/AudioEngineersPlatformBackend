using AudioEngineersPlatformBackend.Application.Abstractions;
using Microsoft.AspNetCore.Http;

namespace AudioEngineersPlatformBackend.Application.Util.Cookies;

public class CookieUtil : ICookieUtil
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CookieUtil(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Task WriteAsCookie(CookieNames cookieNames, string value, DateTime? expirationDate)
    {
        if (expirationDate == null)
        {
            throw new ArgumentNullException($"{nameof(expirationDate)} cannot be null.");
        }

        CookieOptions options = new CookieOptions
        {
            HttpOnly = true,
            Secure = false, // true for HTTPS
            SameSite = SameSiteMode.Strict,
            Expires = expirationDate
        };

        _httpContextAccessor.HttpContext.Response.Cookies.Append(cookieNames.ToString(), value, options);

        return Task.CompletedTask;
    }

    public Task<string> GetCookie(CookieNames cookieNames)
    {
        string? cookieValue = _httpContextAccessor.HttpContext.Request.Cookies[cookieNames.ToString()];

        if (string.IsNullOrWhiteSpace(cookieValue))
        {
            throw new UnauthorizedAccessException("Session expired, please log in again.");
        }

        return Task.FromResult(cookieValue);
    }

    public Task DeleteCookie(CookieNames cookieNames)
    {
        _httpContextAccessor.HttpContext.Response.Cookies.Delete(cookieNames.ToString());
        return Task.CompletedTask;
    }
}