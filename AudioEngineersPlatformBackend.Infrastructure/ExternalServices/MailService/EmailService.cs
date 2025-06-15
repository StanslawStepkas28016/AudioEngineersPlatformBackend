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

    public async Task TrySendVerificationEmailAsync(string toEmail, string firstName, string? verificationCode)
    {
        await using var stream = Assembly
                                     .GetExecutingAssembly()
                                     .GetManifestResourceStream(
                                         "AudioEngineersPlatformBackend.Infrastructure.ExternalServices.Templates.VerificationEmail.html"
                                     )
                                 ?? throw new InvalidOperationException("Could not find embedded resource.");

        using var reader = new StreamReader(stream);
        var template = await reader.ReadToEndAsync();

        var modifiedTemplate = template
            .Replace("{firstName}", firstName)
            .Replace("{verificationCode}", verificationCode);

        // var textBody = $"Hello {userName}, your verification code is {verificationCode}";

        var request = new RestRequest(
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
            // text = textBody,
            html = modifiedTemplate,
            category = "Account Verification"
        });

        var response = await _client.ExecuteAsync(request);

        if (!response.IsSuccessful)
        {
            throw new InvalidOperationException($"Mailtrap Error: {response.StatusCode} / {response.Content}");
        }
    }
}