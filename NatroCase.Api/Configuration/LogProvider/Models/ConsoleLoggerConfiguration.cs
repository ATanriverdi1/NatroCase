namespace NatroCase.Api.Configuration.LogProvider.Models;

public class ConsoleLoggerConfiguration
{
    public int EventId { get; set; }

    public List<LogLevel> AllowedLog { get; set; } = new List<LogLevel>()
    {
        LogLevel.Information,
        LogLevel.Critical,
        LogLevel.Debug,
        LogLevel.Error,
        LogLevel.Trace,
        LogLevel.Warning
    };
}