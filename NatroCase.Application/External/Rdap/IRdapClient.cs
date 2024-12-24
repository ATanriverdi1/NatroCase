using NatroCase.Application.External.Rdap.Models.Response;

namespace NatroCase.Application.External.Rdap;

public interface IRdapClient
{
    Task<CheckDomainNameResponse> CheckDomainNameAsync(string domainName, CancellationToken cancellationToken);
}