using AudioEngineersPlatformBackend.Application.Util.Cookies;

namespace AudioEngineersPlatformBackend.Application.Abstractions;

public interface ICookieUtil
{
    Task WriteAsCookie(CookieNames cookieNames, string value, DateTime? expirationDate);

    Task<string> GetCookie(CookieNames cookieNames);

    Task DeleteCookie(CookieNames cookieNames);
}