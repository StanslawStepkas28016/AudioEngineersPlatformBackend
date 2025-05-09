using AudioEngineersPlatformBackend.Application.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using RestSharp;

namespace AudioEngineersPlatformBackend.Infrastructure.ExternalServices;

public class EmailService : IEmailService
{
    private readonly RestClient _client;
    private readonly string _apiToken;

    public EmailService(IOptions<MailtrapSettings> configuration)
    {
        _client = new RestClient(configuration.Value.RestClientURL);
        _apiToken = configuration.Value.ApiToken;
    }

    public Task Send()
    {
        return Task.CompletedTask;
    }
}