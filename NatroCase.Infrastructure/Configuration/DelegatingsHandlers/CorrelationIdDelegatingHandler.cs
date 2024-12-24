using CorrelationId;
using Microsoft.Extensions.Options;

namespace NatroCase.Infrastructure.Configuration.DelegatingsHandlers;

public class CorrelationIdDelegatingHandler : DelegatingHandler
{
    private readonly ICorrelationContextAccessor _correlationContextAccessor;
    private readonly IOptions<CorrelationIdOptions> _options;
    private static readonly string _headerName = "x-correlationid";
    
    public CorrelationIdDelegatingHandler(
        ICorrelationContextAccessor correlationContextAccessor,
        IOptions<CorrelationIdOptions> options)
    {
        this._options = options;
        this._correlationContextAccessor = correlationContextAccessor;
    }
    
    public CorrelationIdDelegatingHandler(
        ICorrelationContextAccessor correlationContextAccessor,
        IOptions<CorrelationIdOptions> options,
        HttpMessageHandler innerHandler)
        : base(innerHandler)
    {
        this._options = options;
        this._correlationContextAccessor = correlationContextAccessor;
    }
    
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (!request.Headers.Contains(CorrelationIdDelegatingHandler._headerName))
            request.Headers.Add(CorrelationIdDelegatingHandler._headerName, this._correlationContextAccessor?.CorrelationContext?.CorrelationId ?? Guid.NewGuid().ToString());
        return base.SendAsync(request, cancellationToken);
    }
}