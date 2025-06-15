namespace AudioEngineersPlatformBackend.Application.Abstractions;

public interface ICookieUtil
{
    void WriteAsCookie(string cookieName, string token, DateTime? expirationDate);

    string TryGetCookie(string cookieName);

    void DeleteCookie(string cookieName);
}