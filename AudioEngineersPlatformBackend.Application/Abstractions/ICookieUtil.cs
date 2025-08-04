using AudioEngineersPlatformBackend.Application.Util.Cookies;

namespace AudioEngineersPlatformBackend.Application.Abstractions;

public interface ICookieUtil
{
    Task WriteAsCookie(CookieName cookieName, string value, DateTime? expirationDate);

    Task<string> GetCookie(CookieName cookieName);

    Task DeleteCookie(CookieName cookieName);
}