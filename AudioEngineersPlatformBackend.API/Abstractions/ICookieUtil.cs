using API.Util.CookieUtil;

namespace API.Abstractions;

public interface ICookieUtil
{
    Task WriteAsCookie(
        CookieNames cookieNames,
        string value,
        DateTime? expirationDate
    );

    Task<string> GetCookie(
        CookieNames cookieNames
    );

    Task DeleteCookie(
        CookieNames cookieNames
    );
}