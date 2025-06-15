namespace AudioEngineersPlatformBackend.Infrastructure.ExternalServices.MailService;

public class MailtrapSettings
{
    public string RestClientUrl { get; set; }
    public string ApiToken { get; set; }
    public string FromEmail { get; set; }
    public string FromName { get; set; }
}