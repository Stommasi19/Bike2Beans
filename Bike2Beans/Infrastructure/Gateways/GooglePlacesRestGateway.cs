
using Google.Maps.Places.V1;
using Bike2Beans.Application.Common;
using Bike2Beans.Application.CoffeeShops.Queries.Search;
using Bike2Beans.Dtos;
using System.Text.Json;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http.Json;

namespace Bike2Beans.Infrastructure.Gateways;

public sealed class GooglePlacesRestGateway : IPlacesRestGateway
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
    public async Task<GoogleSearchTextResponse> SearchPlacesByTextAsync(
        SearchCoffeeShopByTextQuery query,
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
        var body = new
        {
            textQuery = query.Text,
            pageSize = query.PageSize,
            pageToken = query.PageToken,
            IncludedType = "cafe",
            StrictTypeFiltering = true
        };


        request.Content = JsonContent.Create(body);

        using var response = await _http.SendAsync(request, ct);
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync(ct);

        var googleTextSearchResponse = JsonSerializer.Deserialize<GoogleSearchTextResponse>(responseJson, JsonOptions)
             ?? new GoogleSearchTextResponse();

        return googleTextSearchResponse;

    }
}