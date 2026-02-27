using Microsoft.Extensions.Configuration;

namespace Bike2Beans.Infrastructure.Extensions;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddAppConfiguration(
        this IServiceCollection services,
        IConfiguration config)
    {
        // bind each section to typed options
        // add validation + ValidateOnStart
        return services;
    }
    public static string? GetSetting(this IConfiguration configuration, string envVarName, string configKey)
    {
        var fromEnv = Environment.GetEnvironmentVariable(envVarName);
        if (!string.IsNullOrWhiteSpace(fromEnv))
        {
            return fromEnv;
        }

        var fromConfig = configuration[configKey];
        if (!string.IsNullOrWhiteSpace(fromConfig))
        {
            return fromConfig;
        }

        return null;
    }
    public static string GetRequiredSetting(this IConfiguration configuration, string envVarName, string configKey)
    {
        var fromEnv = Environment.GetEnvironmentVariable(envVarName);
        if (!string.IsNullOrWhiteSpace(fromEnv))
        {
            return fromEnv;
        }

        var fromConfig = configuration[configKey];
        if (!string.IsNullOrWhiteSpace(fromConfig))
        {
            return fromConfig;
        }

        throw new InvalidOperationException($"Missing required configuration value. Set environment variable '{envVarName}' or appsettings key '{configKey}'.");
    }
}