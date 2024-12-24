using System.Net;
using Microsoft.Extensions.Logging;
using NatroCase.Application.External.Rdap;
using NatroCase.Application.External.Rdap.Models.Response;

namespace NatroCase.Infrastructure.Clients;

public class RdapClient : IRdapClient
{
    private readonly HttpClient _client;
    private readonly ILogger<RdapClient> _logger;

    public RdapClient(HttpClient client, ILogger<RdapClient> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<CheckDomainNameResponse> CheckDomainNameAsync(string domainName, CancellationToken cancellationToken)
    {
        var response = await _client.GetAsync($"domain/{domainName}", cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation($"[info] rdap check domainName: {domainName}, statusCode: {response.StatusCode}");
            return new CheckDomainNameResponse(false, domainName);            
        }
        
        if (response is { IsSuccessStatusCode: false, StatusCode: HttpStatusCode.NotFound })
        {
            _logger.LogInformation($"[info] rdap check domainName: {domainName}, statusCode: {response.StatusCode}");
            return new CheckDomainNameResponse(true, domainName);
        }

        var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
        _logger.LogError($"[error] rdap check domainName: {domainName}, statusCode: {response.StatusCode} content: {errorContent}");
        response.EnsureSuccessStatusCode();
        return null;
    }
}