using System.Globalization;
using System.Reflection;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using AudioEngineersPlatformBackend.Application.Abstractions;
using AudioEngineersPlatformBackend.Infrastructure.ExternalServices.MailService.Templates;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace AudioEngineersPlatformBackend.Infrastructure.ExternalServices.SESService;

public class SESService : ISESService
{
    private readonly SESSettings _settings;
    private readonly IAmazonSimpleEmailService _sesClient;
    private readonly IStringLocalizer<EmailMessages> _localizer;

    public SESService(IOptions<SESSettings> settings, IAmazonSimpleEmailService sesClient,
        IStringLocalizer<EmailMessages> localizer)
    {
        _settings = settings.Value;
        _sesClient = sesClient;
        _localizer = localizer;
    }

    public async Task TrySendRegisterVerificationEmailAsync(string toEmail, string firstName,
        string? verificationCode)
    {
        // Validate the parameters - this should never happen but just in case, checking
        if (
            string.IsNullOrWhiteSpace(toEmail)
            || string.IsNullOrWhiteSpace(firstName)
            || string.IsNullOrWhiteSpace(verificationCode)
        )
        {
            throw new ArgumentException("You must provide all arguments to send email messages!");
        }

        // Read the template
        string template = _localizer[EmailMessages.EmailBody];

        // Prepare the template
        var preparedTemplate = template.Replace("{firstName}", firstName)
            .Replace("{verificationCode}", verificationCode);

        // Prepare a request    
        var sendRequest = new SendEmailRequest
        {
            Source = _settings.SenderEmail,
            Destination = new Destination
            {
                ToAddresses =
                    new List<string> { toEmail }
            },
            Message = new Message
            {
                Subject = new Content(_localizer[EmailMessages.EmailSub]),
                Body = new Body
                {
                    Html = new Content
                    {
                        Charset = "UTF-8",
                        Data = preparedTemplate
                    },
                }
            }
        };

        // Send a request
        try
        {
            await _sesClient.SendEmailAsync(sendRequest);
        }
        catch (Exception e)
        {
            throw new Exception($"An error occurred while sending a verification email! {e.Message}");
        }
    }
}