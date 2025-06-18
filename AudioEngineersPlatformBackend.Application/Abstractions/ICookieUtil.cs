using AudioEngineersPlatformBackend.Application.Util.Cookies;

namespace AudioEngineersPlatformBackend.Application.Abstractions;

public interface ICookieUtil
{
    void WriteAsCookie(CookieName cookieName, string value, DateTime? expirationDate);

    string TryGetCookie(CookieName cookieName);

    void DeleteCookie(CookieName cookieName);
}