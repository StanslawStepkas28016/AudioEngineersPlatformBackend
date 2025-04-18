using System.IO;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;

namespace AudioEngineersPlatformBackend.Helpers;

public class SecurityHelpers
{
    private static readonly IConfiguration Configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();

    public static string GenerateVerificationCode()
    {
        var code = RandomNumberGenerator.GetInt32(0, 1000000);
        return code.ToString("D6");
    }
    
    // public static string GenerateRefreshToken()
    // {
    //     var randomNumber = new byte[32];
    //     using var generator = RandomNumberGenerator.Create();
    //     generator.GetBytes(randomNumber);
    //     return Convert.ToBase64String(randomNumber);
    // }
}