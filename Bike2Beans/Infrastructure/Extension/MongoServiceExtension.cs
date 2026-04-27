using Bike2Beans.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Bike2Beans.Infrastructure.Extension;

public static class MongoServiceExtension
{
    public static IServiceCollection AddMongo(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDBSettings>(configuration.GetSection(MongoDBSettings.SectionName));

        services.AddSingleton<IMongoClient>(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<MongoDBSettings>>().Value;
            ValidateMongoSettings(settings);
            return new MongoClient(settings.ConnectionString);
        });

        services.AddSingleton<IMongoDatabase>(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<MongoDBSettings>>().Value;
            ValidateMongoSettings(settings);
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase(settings.DatabaseName);
        });
        return services;
    }

    private static void ValidateMongoSettings(MongoDBSettings settings)
    {
        if (string.IsNullOrWhiteSpace(settings.ConnectionString))
        {
            throw new InvalidOperationException(
                $"Missing required configuration value. Set 'MongoDBSettings:ConnectionString' or environment variable 'MongoDBSettings__ConnectionString'.");
        }

        if (string.IsNullOrWhiteSpace(settings.DatabaseName))
        {
            throw new InvalidOperationException(
                $"Missing required configuration value. Set 'MongoDBSettings:DatabaseName' or environment variable 'MongoDBSettings__DatabaseName'.");
        }

        var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
        var isProduction = string.Equals(environmentName, "Production", StringComparison.OrdinalIgnoreCase);
        if (isProduction && settings.ConnectionString.Contains("localhost", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                "Production MongoDB connection string points at localhost. Set 'MongoDBSettings__ConnectionString' to the production database connection string.");
        }
    }
}
