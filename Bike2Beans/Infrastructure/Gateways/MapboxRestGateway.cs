using Bike2Beans.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Bike2Beans.Application.Common;
using Bike2Beans.Application.CoffeeShops.Commands.Create;
using Bike2Beans.Data;
using Bike2Beans.Dtos;
using Bike2Beans.Models;
using System.Text.Json;

namespace Bike2Beans.Infrastructure.Gateways;


public sealed class MapboxRestGateway : IMapboxRestGateway
{
    private readonly HttpClient _http;
    private readonly string _accessToken;

    public MapboxRestGateway(HttpClient http, IOptions<MapboxOptions> options)
    {
        _http = http;
        _accessToken = options.Value.AccessToken;
        if (string.IsNullOrWhiteSpace(_accessToken))
            throw new InvalidOperationException("Mapbox access token is missing.");
    }


    public async Task<List<RouteOptionDto>> CreateRoute(
        CreateRouteCommand routeinfo,
        CancellationToken ct = default
    )
    {
        var stops = $"{routeinfo.StartLocation[0]},{routeinfo.StartLocation[1]}";
        foreach (var stop in routeinfo.Stops)
        {
            stops += $";{stop.Lat},${stop.Lng}";
        }
        if (routeinfo.EndLocation != null)
        {
            stops += $";{routeinfo.EndLocation[0]},{routeinfo.EndLocation[1]}";
        }
        else
        {
            stops += $";{routeinfo.StartLocation[0]},{routeinfo.StartLocation[1]}";
        }

        var url = $"https://api.mapbox.com/directions/v5/mapbox/cycling/{stops}" +
           $"?alternatives=true&geometries=geojson&steps=true" +
           $"&access_token={_accessToken}";

        var response = await _http.GetAsync(url);

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(ct);

        var mb = JsonSerializer.Deserialize<MapboxRouteResponse>(
        json,
        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
    ) ?? throw new InvalidOperationException("Failed to deserialize Mapbox response.");

        if (mb.Routes.Count == 0)
            return [];
        var options = mb.Routes
    .Select((r, idx) => new RouteOptionDto(
        OptionIndex: idx,
        DistanceMeters: r.Distance,
        DurationSeconds: r.Duration,
        GeometryType: r.Geometry.Type,
        Coordinates: r.Geometry.Coordinates
    )).ToList();

        return options;
    }
}