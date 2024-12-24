using System.Text.Json;
using CorrelationId;
using Microsoft.Extensions.Primitives;
using NatroCase.Api.Configuration.LogProvider.Models;

namespace NatroCase.Api.Configuration.LogProvider;

public class ConsoleLogger : ILogger
{
    private readonly string _name;
    private readonly Func<ConsoleLoggerConfiguration> _getCurrentConfig;
    private readonly ICorrelationContextAccessor _correlationContextAccessor;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ConsoleLogger(
      string name,
      Func<ConsoleLoggerConfiguration> getCurrentConfig,
      ICorrelationContextAccessor correlationContextAccessor,
      IHttpContextAccessor httpContextAccessor)
    {
      this._name = name;
      this._getCurrentConfig = getCurrentConfig;
      this._correlationContextAccessor = correlationContextAccessor;
      this._httpContextAccessor = httpContextAccessor;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
      return (IDisposable) null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
      return this._getCurrentConfig().AllowedLog.Contains(logLevel);
    }

    public void Log<TState>(
      LogLevel logLevel,
      EventId eventId,
      TState state,
      Exception? exception,
      Func<TState, Exception?, string> formatter)
    {
      if (!this.IsEnabled(logLevel))
        return;
      string name = Enum.GetName(logLevel.GetType(), (object) logLevel);
      string environmentVariable1 = Environment.GetEnvironmentVariable("ENABLE_LEVELS");
      if (!string.IsNullOrWhiteSpace(environmentVariable1) && !environmentVariable1.Contains(name))
        return;
      ConsoleLoggerConfiguration loggerConfiguration = this._getCurrentConfig();
      if (loggerConfiguration.EventId != 0 && loggerConfiguration.EventId != eventId.Id)
        return;
      string correlationId = "unknown";
      if (this._correlationContextAccessor?.CorrelationContext?.CorrelationId != null)
        correlationId = this._correlationContextAccessor.CorrelationContext.CorrelationId;
      string agentName = "unknown";
      if (this._httpContextAccessor?.HttpContext?.Request?.Headers != null && this._httpContextAccessor.HttpContext.Request.Headers.ContainsKey("x-agentname"))
        agentName = (string) this._httpContextAccessor.HttpContext.Request.Headers["x-agentname"];
      string domainName = "unknown";
      if (this._httpContextAccessor?.HttpContext?.Request?.Headers != null)
      {
        KeyValuePair<string, StringValues> keyValuePair = this._httpContextAccessor.HttpContext.Request.Headers.FirstOrDefault(x => x.Key.ToLower() == "domainname");
        if (!string.IsNullOrEmpty((string) keyValuePair.Value))
          domainName = (string) keyValuePair.Value;
      }
      string message = exception == null ? formatter(state, exception) : exception.Message + " (inner ex: " + exception.InnerException?.Message + ")";
      string machineName = Environment.MachineName;
      string environmentVariable2 = Environment.GetEnvironmentVariable("SERVICE_NAME");
      string environmentVariable3 = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
      string str = DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss.fffffffK");
      Console.WriteLine(JsonSerializer.Serialize<LogModel>(new LogModel(name, correlationId, agentName, environmentVariable2, this._name, environmentVariable3, machineName, message, str, str, exception?.StackTrace, domainName), new JsonSerializerOptions()
      {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
      }) ?? "");
    }
}