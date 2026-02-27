using Bike2Beans.Options;
using Bike2Beans.Application.Common;

namespace Bike2Beans.Infrastructure;


public static class MapboxServiceExtension
{
    public static IServiceCollection AddMapbox(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MapboxOptions>(
            configuration.GetSection(MapboxOptions.SectionName));

        services.AddHttpClient<MapboxRestGateway>();

        return services;
    }
}