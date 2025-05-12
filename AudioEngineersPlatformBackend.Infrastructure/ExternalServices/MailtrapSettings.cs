namespace AudioEngineersPlatformBackend.Infrastructure.ExternalServices;

public class MailtrapSettings
{
    public string RestClientURL { get; set; }
    public string ApiToken { get; set; }
    public string FromEmail { get; set; }
    public string FromName { get; set; }
}