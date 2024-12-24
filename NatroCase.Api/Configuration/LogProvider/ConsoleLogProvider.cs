using System.Collections.Concurrent;
using CorrelationId;
using Microsoft.Extensions.Options;
using NatroCase.Api.Configuration.LogProvider.Models;

namespace NatroCase.Api.Configuration.LogProvider;

public class ConsoleLogProvider : ILoggerProvider, IDisposable
{
    private readonly IDisposable? _onChangeToken;
    private ConsoleLoggerConfiguration _currentConfig;
    private readonly ICorrelationContextAccessor _correlationContextAccessor;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ConcurrentDictionary<string, ConsoleLogger> _loggers = new ConcurrentDictionary<string, ConsoleLogger>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);

    public ConsoleLogProvider(
        IOptionsMonitor<ConsoleLoggerConfiguration> config,
        ICorrelationContextAccessor correlationContextAccessor,
        IHttpContextAccessor httpContextAccessor)
    {
        this._correlationContextAccessor = correlationContextAccessor;
        this._httpContextAccessor = httpContextAccessor;
        this._currentConfig = config.CurrentValue;
        this._onChangeToken = config.OnChange<ConsoleLoggerConfiguration>((Action<ConsoleLoggerConfiguration>) (updatedConfig => this._currentConfig = updatedConfig));
    }

    private ConsoleLoggerConfiguration GetCurrentConfig() => this._currentConfig;

    public void Dispose()
    {
        this._loggers.Clear();
        this._onChangeToken?.Dispose();
    }

    public ILogger CreateLogger(string categoryName)
    {
        return (ILogger) this._loggers.GetOrAdd(categoryName, (Func<string, ConsoleLogger>) (name => new ConsoleLogger(name, new Func<ConsoleLoggerConfiguration>(this.GetCurrentConfig), this._correlationContextAccessor, this._httpContextAccessor)));
    }
}