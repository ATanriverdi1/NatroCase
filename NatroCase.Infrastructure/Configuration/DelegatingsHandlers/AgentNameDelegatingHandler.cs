using Microsoft.Extensions.Configuration;

namespace NatroCase.Infrastructure.Configuration.DelegatingsHandlers;

public class AgentNameDelegatingHandler : DelegatingHandler
{
    private static readonly string _headerName = "x-agentname";
    private readonly string _agentName;
    
    public AgentNameDelegatingHandler(IConfiguration configuration)
    {
        this._agentName = configuration["AgentName"];
    }

    public AgentNameDelegatingHandler(HttpMessageHandler innerHandler, IConfiguration configuration)
        : base(innerHandler)
    {
        this._agentName = configuration["AgentName"];
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (!request.Headers.Contains(AgentNameDelegatingHandler._headerName))
            request.Headers.Add(AgentNameDelegatingHandler._headerName, (IEnumerable<string>) new List<string>()
            {
                this._agentName
            });
        return base.SendAsync(request, cancellationToken);
    }
    
}