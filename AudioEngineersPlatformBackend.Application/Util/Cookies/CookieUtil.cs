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

    public void WriteAsCookie(CookieName cookieName, string value, DateTime? expirationDate)
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

        _httpContextAccessor.HttpContext.Response.Cookies.Append(cookieName.ToString(), value, options);
    }

    public string TryGetCookie(CookieName cookieName)
    {
        string? cookieValue = _httpContextAccessor.HttpContext.Request.Cookies[cookieName.ToString()];

        if (string.IsNullOrWhiteSpace(cookieValue))
        {
            throw new UnauthorizedAccessException("Session expired, please log in again.");
        }

        return cookieValue;
    }

    public void DeleteCookie(CookieName cookieName)
    {
        _httpContextAccessor.HttpContext.Response.Cookies.Delete(cookieName.ToString());
    }
}