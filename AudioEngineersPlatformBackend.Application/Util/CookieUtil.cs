using AudioEngineersPlatformBackend.Application.Abstractions;
using Microsoft.AspNetCore.Http;

namespace AudioEngineersPlatformBackend.Application.Util;

public class CookieUtil : ICookieUtil
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CookieUtil(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void WriteAsCookie(string cookieName, string token, DateTime? expirationDate)
    {
        if (string.IsNullOrWhiteSpace(cookieName) || string.IsNullOrWhiteSpace(token))
        {
            throw new Exception("All parameters (cookieName, token) must be provided.");
        }

        var options = new CookieOptions
        {
            HttpOnly = true,
            Secure = false, // true for HTTPS
            SameSite = SameSiteMode.Strict,
            Expires = expirationDate
        };

        _httpContextAccessor.HttpContext.Response.Cookies.Append(cookieName, token, options);
    }

    public string TryGetCookie(string cookieName)
    {
        if (string.IsNullOrWhiteSpace(cookieName))
        {
            throw new Exception("Cookie name must be provided.");
        }

        var cookie = _httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];

        if (string.IsNullOrWhiteSpace(cookie))
        {
            throw new Exception("No refresh token cookie found.");
        }

        return cookie;
    }

    public void DeleteCookie(string cookieName)
    {
        if (string.IsNullOrWhiteSpace(cookieName))
        {
            throw new Exception("Cookie name must be provided.");
        }

        _httpContextAccessor.HttpContext.Response.Cookies.Delete(cookieName);
    }
}