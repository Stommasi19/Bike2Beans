using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Bike2Beans.Models;
using System.Text.Json;
using Bike2Beans.Domain.Interfaces;
using Bike2Beans.Infrastructure.Responses;
using Bike2Beans.Domain.DTOs;
using Bike2Beans.Domain.Extension;

namespace Bike2Beans.Infrastructure.Gateways;



public sealed class MapboxRestGateway : IRouteProvider
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
        List<double> startLocation,
        List<double>? endLocation,
        List<CoffeeshopDto> stops,
        CancellationToken ct = default
    )
    {
        var stopsstring = $"{startLocation[0]},{startLocation[1]}";
        foreach (var stop in stops)
        {
            stopsstring += $";{stop.Lat},{stop.Lng}";
        }
        if (endLocation != null || endLocation.Count != 0)
        {
            stopsstring += $";{endLocation[0]},{endLocation[1]}";
        }
        else
        {
            stopsstring += $";{startLocation[0]},{startLocation[1]}";
        }

        var url = $"https://api.mapbox.com/directions/v5/mapbox/cycling/{stopsstring}" +
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