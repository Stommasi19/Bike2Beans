using Bike2Beans.Infrastructure.Extensions;
using Bike2Beans.Infrastructure.Gateways;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;


namespace Bike2Beans.Infrastructure.Extension;

public class MapboxOptions
{
    public const string SectionName = "Mapbox";

    public string AccessToken { get; init; } = "";
}
public static class MapboxServiceExtension
{
    private const string AccessTokenEnvVarName = "MAPBOX_ACCESS_TOKEN";
    private static readonly string AccessTokenConfigKey = $"{MapboxOptions.SectionName}:AccessToken";

    public static IServiceCollection AddMapbox(this IServiceCollection services, IConfiguration configuration)
    {
        var accessToken = configuration.GetRequiredSetting(AccessTokenEnvVarName, AccessTokenConfigKey);
        if (!accessToken.StartsWith("pk.", StringComparison.OrdinalIgnoreCase)
            && !accessToken.StartsWith("sk.", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                $"Invalid Mapbox token format. Value from '{AccessTokenEnvVarName}'/'{AccessTokenConfigKey}' must start with 'pk.' or 'sk.'.");
        }

        services.AddSingleton<IOptions<MapboxOptions>>(
            Microsoft.Extensions.Options.Options.Create(new MapboxOptions
            {
                AccessToken = accessToken
            }));

        services.AddHttpClient<MapboxRestGateway>();

        return services;
    }
}
