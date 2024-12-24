using System.Text.Json.Serialization;

namespace NatroCase.Api.Configuration.LogProvider.Models;

public class LogModel
{
    public LogModel(
        string? severity,
        string? correlationId,
        string? agentName,
        string? app,
        string? service,
        string? env,
        string? host,
        string? message,
        string? date,
        string? timestamp,
        string? stackTrace,
        string? domainName)
    {
        this.Severity = severity;
        this.CorrelationId = correlationId;
        this.AgentName = agentName;
        this.App = app;
        this.Service = service;
        this.Env = env;
        this.Host = host;
        this.Message = message;
        this.Date = date;
        this.Timestamp = timestamp;
        this.StackTrace = stackTrace;
        this.DomainName = domainName;
    }

    public string? CorrelationId { get; set; }

    public string? AgentName { get; set; }

    public string? Severity { get; set; }

    public string? App { get; set; }

    public string? Service { get; set; }

    public string? Env { get; set; }

    public string? Host { get; set; }

    public string? Message { get; set; }

    public string? StackTrace { get; set; }

    public string? Date { get; set; }

    [JsonPropertyName("@timestamp")]
    public string? Timestamp { get; set; }

    public string? DomainName { get; set; }
}