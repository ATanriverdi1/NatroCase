using MediatR;
using NatroCase.Application.External.Rdap.Models.Response;

namespace NatroCase.Application.External.Rdap.Queries;

public record RdapCheckDomainNameQuery(string DomainName) : IRequest<CheckDomainNameResponse>
{
    public sealed class Handler : IRequestHandler<RdapCheckDomainNameQuery, CheckDomainNameResponse>
    {
        private readonly IRdapClient _rdapClient;

        public Handler(IRdapClient rdapClient)
        {
            _rdapClient = rdapClient;
        }

        public async Task<CheckDomainNameResponse> Handle(RdapCheckDomainNameQuery request, CancellationToken cancellationToken)
        {
            var domainName = ExtractDomainName(request.DomainName);
            var checkDomainResponse = await _rdapClient.CheckDomainNameAsync(domainName, cancellationToken);
            return checkDomainResponse;
        }
        
        private static string ExtractDomainName(string domainName)
        {
            try
            {
                var uri = new UriBuilder(domainName).Uri;
                return uri.Host;
            }
            catch
            {
                return domainName.Trim().ToLower();
            }
        }
    }
}