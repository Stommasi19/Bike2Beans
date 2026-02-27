using Bike2Beans.Options;
using Google.Api.Gax.Grpc.Rest;
using Google.Maps.Places.V1;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Bike2Beans.Application.Common;
using Bike2Beans.Infrastructure.Extensions;



namespace Bike2Beans.Infrastructure;

public static class GoogleServiceExtension
{
    private const string ApiKeyEnvVarName = "GOOGLE_PLACES_API_KEY";
    private static readonly string ApiKeyConfigKey = $"{GooglePlacesOptions.SectionName}:ApiKey";

    public static IServiceCollection AddGooglePlaces(this IServiceCollection services, IConfiguration configuration)
    {
        var apiKey = configuration.GetRequiredSetting(ApiKeyEnvVarName, ApiKeyConfigKey);
        if (apiKey.Trim().Length < 20)
        {
            throw new InvalidOperationException(
                $"Invalid Google Places API key. Value from '{ApiKeyEnvVarName}'/'{ApiKeyConfigKey}' appears too short.");
        }

        services.AddSingleton<IOptions<GooglePlacesOptions>>(
            Microsoft.Extensions.Options.Options.Create(new GooglePlacesOptions
            {
                ApiKey = apiKey
            }));

        services.AddSingleton<PlacesClient>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<GooglePlacesOptions>>().Value;

            var builder = new PlacesClientBuilder
            {
                ApiKey = options.ApiKey,
                GrpcAdapter = RestGrpcAdapter.Default
            };
            return builder.Build();
        });

        services.AddHttpClient<IPlacesRestGateway, GooglePlacesRestGateway>((sp, client) =>
        {
            var options = sp.GetRequiredService<IOptions<GooglePlacesOptions>>().Value;

            client.DefaultRequestHeaders.Add("X-Goog-Api-Key", options.ApiKey);
        });

        return services;
    }
}
