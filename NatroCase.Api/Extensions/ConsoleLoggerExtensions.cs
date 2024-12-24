using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging.Configuration;
using NatroCase.Api.Configuration.LogProvider;
using NatroCase.Api.Configuration.LogProvider.Models;

namespace NatroCase.Api.Extensions;

public static class ConsoleLoggerExtensions
{
    public static ILoggingBuilder AddConsoleLogger(this ILoggingBuilder builder)
    {
        builder.AddConfiguration();
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, ConsoleLogProvider>());
        LoggerProviderOptions.RegisterProviderOptions<ConsoleLoggerConfiguration, ConsoleLogProvider>(builder.Services);
        return builder;
    }

    public static ILoggingBuilder AddConsoleLogger(
        this ILoggingBuilder builder,
        Action<ConsoleLoggerConfiguration> configure)
    {
        builder.AddConsoleLogger();
        builder.Services.Configure<ConsoleLoggerConfiguration>(configure);
        return builder;
    }
}