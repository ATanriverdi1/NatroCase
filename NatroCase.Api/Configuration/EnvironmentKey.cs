namespace NatroCase.Api.Configuration;

public static class EnvironmentKey
{
    private static List<string> _definedEnvironments = new List<string>()
    {
        "local",
        "development"
    };
    
    public static string FileExtension
    {
        get
        {
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (environment != null && environment.ToLower().StartsWith("prod"))
                return ".production";
            string str = EnvironmentKey._definedEnvironments.FirstOrDefault<string>((Func<string, bool>) (p => p == environment));
            return str != null ? "." + str : string.Empty;
        }
    }
    
    public static bool IsEnvironmentConfigsEnabled
    {
        get
        {
            string environmentVariable = Environment.GetEnvironmentVariable("ENV_CONFIGS_ENABLED");
            return !string.IsNullOrEmpty(environmentVariable) && bool.Parse(environmentVariable);
        }
    }

    
    public static bool IsDevelopment() => EnvironmentKey.GetCurrentEnvironment() == "development";
    public static bool IsProduction() => EnvironmentKey.GetCurrentEnvironment() == "production";
    
    public static string GetCurrentEnvironment()
    {
        string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        if (environment != null)
        {
            if (environment.ToLower().StartsWith("prod"))
                return "production";
            string currentEnvironment = EnvironmentKey._definedEnvironments.FirstOrDefault<string>((Func<string, bool>) (p => p.Equals(environment, StringComparison.OrdinalIgnoreCase)));
            if (currentEnvironment != null)
                return currentEnvironment;
        }
        return "development";
    }
}