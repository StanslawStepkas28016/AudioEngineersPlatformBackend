using System.Globalization;
using System.Text.RegularExpressions;

namespace AudioEngineersPlatformBackend.Domain.ValueObjects;

public readonly struct EmailVo
{
    private readonly string? _address;

    private bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            // Normalize the domain
            email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                RegexOptions.None, TimeSpan.FromMilliseconds(200));

            // Examines the domain part of the email and normalizes it.
            string DomainMapper(Match match)
            {
                // Use IdnMapping class to convert Unicode domain names.
                IdnMapping idn = new IdnMapping();

                // Pull out and process domain name (throws ArgumentException on invalid)
                string domainName = idn.GetAscii(match.Groups[2].Value);

                return match.Groups[1].Value + domainName;
            }
        }
        catch (RegexMatchTimeoutException e)
        {
            return false;
        }
        catch (ArgumentException e)
        {
            return false;
        }

        try
        {
            return Regex.IsMatch(email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }

    private string Address
    {
        get { return _address; }
        init
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    $"Provided {nameof(Address)} from {nameof(GetType)} cannot be null or empty.");
            }

            if (!IsValidEmail(value))
            {
                throw new ArgumentException($"Provided {nameof(Address)} is not valid, needs to follow name@domain.com.");
            }

            _address = value;
        }
    }

    public EmailVo(string address)
    {
        Address = address;
    }

    public string GetValidEmail()
    {
        return _address;
    }
}