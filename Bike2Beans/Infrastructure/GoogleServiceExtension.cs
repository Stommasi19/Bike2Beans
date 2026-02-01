using Bike2Beans.Options;
using Google.Api.Gax.Grpc.Rest;
using Google.Maps.Places.V1;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;


namespace Bike2Beans.Infrastructure;

public static class GoogleServiceExtension
{
    public static IServiceCollection AddGooglePlaces(this IServiceCollection services, IConfiguration configuration)
    {

        services
            .AddOptions<GooglePlacesOptions>()
            .Bind(configuration.GetSection("GooglePlaces"))
            .Validate(o => !string.IsNullOrWhiteSpace(o.ApiKey), "GooglePlaces:ApiKey is required")
            .ValidateOnStart();

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
        return services;
    }
}