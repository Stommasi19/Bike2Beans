using Microsoft.Extensions.Configuration;

namespace Bike2Beans.Infrastructure.Extensions;

public static class ConfigurationExtensions
{
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

    public static bool GetRequiredBool(this IConfiguration configuration, string envVarName, string configKey)
    {
        var value = configuration.GetRequiredSetting(envVarName, configKey);
        if (bool.TryParse(value, out var boolValue))
        {
            return boolValue;
        }

        if (int.TryParse(value, out var intValue))
        {
            return intValue != 0;
        }

        throw new InvalidOperationException($"Configuration value for '{envVarName}'/'{configKey}' must be a boolean.");
    }

    public static int GetRequiredInt(this IConfiguration configuration, string envVarName, string configKey)
    {
        var value = configuration.GetRequiredSetting(envVarName, configKey);
        if (int.TryParse(value, out var intValue))
        {
            return intValue;
        }

        throw new InvalidOperationException($"Configuration value for '{envVarName}'/'{configKey}' must be an integer.");
    }

    public static Guid GetRequiredGuid(this IConfiguration configuration, string envVarName, string configKey)
    {
        var value = configuration.GetRequiredSetting(envVarName, configKey);
        if (Guid.TryParse(value, out var guidValue))
        {
            return guidValue;
        }

        throw new InvalidOperationException($"Configuration value for '{envVarName}'/'{configKey}' must be a GUID.");
    }

    public static string[] GetRequiredCsv(this IConfiguration configuration, string envVarName, string configKey, char separator = ',')
    {
        var value = configuration.GetRequiredSetting(envVarName, configKey);
        return value
            .Split(separator, StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Trim())
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .ToArray();
    }

    public static T? GetSectionAs<T>(this IConfiguration configuration, string envVarName, string sectionKey) where T : class
    {
        var section = configuration.GetSection(sectionKey);
        if (section.Exists())
        {
            return section.Get<T>();
        }

        return null;
    }

    public static int GetIntOrDefault(this IConfiguration configuration, string envVarName, string configKey, int defaultValue)
    {
        var value = configuration.GetSetting(envVarName, configKey);
        if (string.IsNullOrWhiteSpace(value))
        {
            return defaultValue;
        }

        if (int.TryParse(value, out var intValue))
        {
            return intValue;
        }

        return defaultValue;
    }

    public static bool GetBoolOrDefault(this IConfiguration configuration, string envVarName, string configKey, bool defaultValue)
    {
        var value = configuration.GetSetting(envVarName, configKey);
        if (string.IsNullOrWhiteSpace(value))
        {
            return defaultValue;
        }

        if (bool.TryParse(value, out var boolValue))
        {
            return boolValue;
        }

        if (int.TryParse(value, out var intValue))
        {
            return intValue != 0;
        }

        return defaultValue;
    }
}
