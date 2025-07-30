using System.Reflection;
using AudioEngineersPlatformBackend.Application.Abstractions;
using Microsoft.Extensions.Options;
using RestSharp;

namespace AudioEngineersPlatformBackend.Infrastructure.ExternalServices.MailService;

public class EmailService : IEmailService
{
    private readonly RestClient _client;
    private readonly string _apiToken;
    private readonly string _fromEmail;
    private readonly string _fromName;

    public EmailService(IOptions<MailtrapSettings> configuration)
    {
        _client = new RestClient(configuration.Value.RestClientUrl);
        _apiToken = configuration.Value.ApiToken;
        _fromEmail = configuration.Value.FromEmail;
        _fromName = configuration.Value.FromName;
    }

    /// <summary>
    ///     Method used for sending a verification email
    /// </summary>
    /// <param name="toEmail"></param>
    /// <param name="firstName"></param>
    /// <param name="verificationCode"></param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task TrySendRegisterVerificationEmailAsync(string toEmail, string firstName, string? verificationCode)
    {
        // Validate the parameters - this should never happen but just in case, checking
        if (string.IsNullOrWhiteSpace(toEmail)
            || string.IsNullOrWhiteSpace(firstName)
            || string.IsNullOrWhiteSpace(verificationCode))
        {
            throw new ArgumentException("You must provide all arguments to send email messages!");
        }

        // Get the template from files
        await using Stream stream = Assembly
                                        .GetExecutingAssembly()
                                        .GetManifestResourceStream(
                                            "AudioEngineersPlatformBackend.Infrastructure.ExternalServices.MailService.Templates.VerificationEmail.html"
                                        )
                                    ?? throw new InvalidOperationException("Could not find embedded resource.");

        // Read the template
        using StreamReader reader = new StreamReader(stream);
        string template = await reader.ReadToEndAsync();

        // Prepare the template
        string modifiedTemplate = template
            .Replace("{firstName}", firstName)
            .Replace("{verificationCode}", verificationCode);


        // Prepare a request to the external mailing API
        RestRequest request = new RestRequest(
            resource: "",
            method: Method.Post
        );
        request.AddHeader("Authorization", $"Bearer {_apiToken}");
        request.AddHeader("Content-Type", "application/json");
        request.AddJsonBody(new
        {
            from = new { email = _fromEmail, name = _fromName },
            to = new[] { new { email = toEmail } },
            subject = "Here is your verification code :)",
            html = modifiedTemplate,
            category = "Account Verification"
        });

        // Execute the request
        RestResponse response = await _client.ExecuteAsync(request);

        // Check if the request was successful
        if (!response.IsSuccessful)
        {
            throw new InvalidOperationException($"Mailtrap Error: {response.StatusCode} / {response.Content}");
        }
    }

    /// <summary>
    ///     Method used for sending a password change email.
    ///     Though similar to the "TrySendRegisterVerificationEmailAsync",
    ///     it is there to explicitly suggest that it does something different
    /// </summary>
    /// <param name="toEmail"></param>
    /// <param name="firstName"></param>
    /// <param name="verificationCode"></param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task TrySendChangeEmailEmailAsync(string toEmail, string firstName, string? verificationCode)
    {
        // Validate the parameters - this should never happen but just in case, checking
        if (string.IsNullOrWhiteSpace(toEmail)
            || string.IsNullOrWhiteSpace(firstName)
            || string.IsNullOrWhiteSpace(verificationCode))
        {
            throw new ArgumentException("You must provide all arguments to send email messages!");
        }

        // Get the template from files
        await using Stream stream = Assembly
                                        .GetExecutingAssembly()
                                        .GetManifestResourceStream(
                                            "AudioEngineersPlatformBackend.Infrastructure.ExternalServices.MailService.Templates.EmailChangeEmail.html"
                                        )
                                    ?? throw new InvalidOperationException("Could not find embedded resource.");

        // Read the template
        using StreamReader reader = new StreamReader(stream);
        string template = await reader.ReadToEndAsync();

        // Prepare the template
        string modifiedTemplate = template
            .Replace("{firstName}", firstName)
            .Replace("{verificationCode}", verificationCode);


        // Prepare a request to the external mailing API
        RestRequest request = new RestRequest(
            resource: "",
            method: Method.Post
        );
        request.AddHeader("Authorization", $"Bearer {_apiToken}");
        request.AddHeader("Content-Type", "application/json");
        request.AddJsonBody(new
        {
            from = new { email = _fromEmail, name = _fromName },
            to = new[] { new { email = toEmail } },
            subject = "You have requested an email change",
            html = modifiedTemplate,
            category = "Email Change Request"
        });

        // Execute the request
        RestResponse response = await _client.ExecuteAsync(request);

        // Check if the request was successful
        if (!response.IsSuccessful)
        {
            throw new InvalidOperationException($"Mailtrap Error: {response.StatusCode} / {response.Content}");
        }
    }
}