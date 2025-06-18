using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using AudioEngineersPlatformBackend.Application.Abstractions;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Shared.Resources;

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

    private async Task SendEmailAsync(SendEmailRequest sendEmailRequest)
    {
        // Send a request
        try
        {
            await _sesClient.SendEmailAsync(sendEmailRequest);
        }
        catch (Exception e)
        {
            throw new Exception
            (
                $"An error occurred while sending the {nameof(sendEmailRequest)}, please contact the administrator. {e.Message}"
            );
        }
    }

    public async Task SendRegisterVerificationEmailAsync(string toEmail, string firstName,
        string? verificationCode)
    {
        // Validate the parameters - this should never happen but just in case, checking
        if (
            string.IsNullOrWhiteSpace(toEmail)
            || string.IsNullOrWhiteSpace(firstName)
            || string.IsNullOrWhiteSpace(verificationCode)
        )
        {
            throw new ArgumentException("You must provide all arguments to send email messages.");
        }

        // Read the template
        string bodyTemplate = _localizer[EmailMessages.VerificationEmailBody];

        // Prepare the template
        var preparedBodyTemplate = bodyTemplate.Replace("{firstName}", firstName)
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
                Subject = new Content(_localizer[EmailMessages.VerificationEmailSubject]),
                Body = new Body
                {
                    Html = new Content
                    {
                        Charset = "UTF-8",
                        Data = preparedBodyTemplate
                    },
                }
            }
        };

        await SendEmailAsync(sendRequest);
    }

    public async Task SendEmailResetEmailAsync(string toEmail, string firstName, string uniqueUrl)
    {
        // Validate the parameters - this should never happen but just in case, checking
        if (
            string.IsNullOrWhiteSpace(toEmail)
            || string.IsNullOrWhiteSpace(firstName)
            || string.IsNullOrWhiteSpace(uniqueUrl)
        )
        {
            throw new ArgumentException("You must provide all arguments to send email messages.");
        }

        // Read the template
        string bodyTemplate = _localizer[EmailMessages.ResetEmailEmailBody];

        // Prepare the template
        var preparedBodyTemplate = bodyTemplate.Replace("{firstName}", firstName)
            .Replace("{uniqueUrl}", uniqueUrl);


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
                Subject = new Content(_localizer[EmailMessages.ResetEmailEmailSubject]),
                Body = new Body
                {
                    Html = new Content
                    {
                        Charset = "UTF-8",
                        Data = preparedBodyTemplate
                    },
                }
            }
        };

        await SendEmailAsync(sendRequest);
    }

    public async Task SendPasswordResetEmailAsync(string toEmail, string firstName, string uniqueUrl)
    {
        // Validate the parameters - this should never happen but just in case, checking
        if (
            string.IsNullOrWhiteSpace(toEmail)
            || string.IsNullOrWhiteSpace(firstName)
            || string.IsNullOrWhiteSpace(uniqueUrl)
        )
        {
            throw new ArgumentException("You must provide all arguments to send email messages.");
        }

        // Read the template
        string bodyTemplate = _localizer[EmailMessages.ResetPasswordEmailBody];


        // Prepare the template
        var preparedBodyTemplate = bodyTemplate.Replace("{firstName}", firstName)
            .Replace("{uniqueUrl}", uniqueUrl);


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
                Subject = new Content(_localizer[EmailMessages.ResetPasswordEmailSubject]),
                Body = new Body
                {
                    Html = new Content
                    {
                        Charset = "UTF-8",
                        Data = preparedBodyTemplate
                    },
                }
            }
        };

        await SendEmailAsync(sendRequest);
    }
}