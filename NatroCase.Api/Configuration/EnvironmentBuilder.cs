using NatroCase.Api.Extensions;

namespace NatroCase.Api.Configuration;

public class EnvironmentBuilder
{
    public static void ConfigureConfiguration(IConfigurationBuilder config)
    {
        config.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings" + EnvironmentKey.FileExtension + ".json", false, true);
        if (!EnvironmentKey.IsEnvironmentConfigsEnabled)
            return;
        config.AddEnvironmentVariables();
    }

    public static void ConfigureLogging(ILoggingBuilder builder)
    {
        builder.ClearProviders();
        builder.AddConsoleLogger();
    }
}