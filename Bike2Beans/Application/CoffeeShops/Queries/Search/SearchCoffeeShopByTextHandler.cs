using Bike2Beans.Dtos;
using Google.Maps.Places.V1;
using Google.Protobuf.WellKnownTypes;
using Google.Type;
using Google.Api.Gax.Grpc;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Bike2Beans.Infrastructure;
using Bike2Beans.Application.Common;

namespace Bike2Beans.Application.CoffeeShops.Queries.Search;

public class SearchCoffeeShopByTextHandler
{
    private readonly PlacesClient _places;
    public SearchCoffeeShopByTextHandler(PlacesClient places)
    {
        _places = places;
    }

    public async Task<List<CoffeeShopDto>> Handle(
        SearchCoffeeShopByTextQuery query,
        CancellationToken ct = default
    )
    {
        var fieldMask = "places.id,places.displayName,places.formattedAddress,places.location,places.rating,places.userRatingCount,nextPageToken"; var callSettings = CallSettings
        .FromHeader("X-Goog-FieldMask", fieldMask);




        var request = new SearchTextRequest
        {
            TextQuery = query.Text,
            // PageToken = query.PageToken ?? "",
            MaxResultCount = query.PageSize,
            IncludedType = "cafe",
            StrictTypeFiltering = true
        };

        var response = await _places.SearchTextAsync(request, callSettings);

        var result = response.Places.Select(p => new CoffeeShopDto(
            p.Id,
            p.DisplayName?.Text ?? "",
            p.FormattedAddress,
            p.Rating,
            p.UserRatingCount,
            p.Location.Latitude,
            p.Location.Longitude
            )).ToList();

        return result;
        // return new PagedResult<CoffeeShopDto>(result, response.NextPageToken ?? null);
    }
}