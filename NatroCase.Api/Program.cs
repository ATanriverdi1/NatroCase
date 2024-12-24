using NatroCase.Api.Configuration;

namespace NatroCase.Api;
public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(EnvironmentBuilder.ConfigureConfiguration)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureLogging(EnvironmentBuilder.ConfigureLogging);
                webBuilder.UseStartup<Startup>();
            });
}