using NatroCase.Api.Configuration;
using NatroCase.Application.External.Rdap;
using NatroCase.Infrastructure.Clients;
using NatroCase.Infrastructure.Configuration.DelegatingsHandlers;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;

namespace NatroCase.Api.Extensions;

public static class ServiceCollectionHttpClientExtensions
{
    public static IServiceCollection AddHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<CorrelationIdDelegatingHandler>();
        services.AddTransient<AgentNameDelegatingHandler>();    
        services.AddHttpClient<IRdapClient, RdapClient>(configuration, configuration.GetValue<string>(ConfigKeys.RdapClientUrl));
        return services;
    }
    
    public static IServiceCollection AddHttpClient<TClient, TImplementation>(this IServiceCollection services,
        string baseAddress, int timeoutInMs, int retryCount, int retryIntervalInMs)
        where TClient : class
        where TImplementation : class, TClient
    {
        services.AddHttpClient<TClient, TImplementation>(client => client.BaseAddress = new Uri(baseAddress))
            .AddHttpMessageHandler<AgentNameDelegatingHandler>()
            .AddHttpMessageHandler<CorrelationIdDelegatingHandler>()
            .AddPolicyHandler(
                Policy.TimeoutAsync<HttpResponseMessage>(
                    TimeSpan.FromMilliseconds(timeoutInMs)))
            .AddPolicyHandler(HttpPolicyExtensions.HandleTransientHttpError()
                .Or<TimeoutRejectedException>()
                .WaitAndRetryAsync(retryCount,
                    _ => TimeSpan.FromMilliseconds(retryIntervalInMs)))
            .SetHandlerLifetime(TimeSpan.FromMinutes(30));

        return services;
    }

    public static IServiceCollection AddHttpClient<TClient, TImplementation>(this IServiceCollection services, IConfiguration configuration, string baseAddress)
        where TClient : class
        where TImplementation : class, TClient
    {
        return services.AddHttpClient<TClient, TImplementation>(
            baseAddress,
            configuration.GetValue<int>(ConfigKeys.HttpClientTimeoutInMs),
            configuration.GetValue<int>(ConfigKeys.HttpClientRetryCount),
            configuration.GetValue<int>(ConfigKeys.HttpClientRetryIntervalInMs)
        );
    }
}