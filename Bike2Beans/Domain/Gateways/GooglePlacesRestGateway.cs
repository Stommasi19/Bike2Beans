
using Google.Maps.Places.V1;
using System.Text.Json;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http.Json;
using Bike2Beans.Infrastructure.Responses;
using Bike2Beans.Domain.Interfaces;
using Bike2Beans.Domain.DTOs;
using Bike2Beans.Domain.Entities;

namespace Bike2Beans.Domain.Gateways;

public class GooglePlacesOptions
{
    public const string SectionName = "GooglePlaces";

    public string ApiKey { get; init; } = "";
}
public sealed class GooglePlacesRestGateway : ILocationProvider
{
    private readonly HttpClient _http;
    public GooglePlacesRestGateway(HttpClient http)
    {
        _http = http;
    }
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };
    public async Task<LocationPaginatedResponse> SearchPlacesByTextAsync(
        string text,
        int pageSize,
        string? pageToken = null,
        bool coffeeOnly = true,
        CancellationToken ct = default
        )
    {

        var request = new HttpRequestMessage(
            HttpMethod.Post,
            "https://places.googleapis.com/v1/places:searchText"
        );
        request.Headers.Add("X-Goog-FieldMask",
                "places.id,places.displayName,places.formattedAddress,places.location,places.rating,places.userRatingCount,nextPageToken"
                );
        object body = new
        {
            textQuery = text,
            pageSize = pageSize,
            pageToken = pageToken
        };

        if (coffeeOnly)
        {
            body = new
            {
                textQuery = text,
                pageSize = pageSize,
                pageToken = pageToken,
                IncludedType = "cafe",
                StrictTypeFiltering = true
            };
        }


        request.Content = JsonContent.Create(body);

        using var response = await _http.SendAsync(request, ct);
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync(ct);

        var googleTextSearchResponse = JsonSerializer.Deserialize<GoogleSearchTextResponse>(responseJson, JsonOptions)
             ?? new GoogleSearchTextResponse();

        var returnResponse = new LocationPaginatedResponse
        {
            NextPageToken = googleTextSearchResponse.NextPageToken,
            Locations = googleTextSearchResponse.Places.Select(loc => new CoffeeshopDto(
                loc.Id ?? "",
                loc.DisplayName?.Text ?? "",
                loc.FormattedAddress,
                loc.Rating,
                loc.UserRatingCount,
                loc.Location?.Latitude ?? 0,
                loc.Location?.Longitude ?? 0
            )).ToList()
        };
        return returnResponse;
    }

}
