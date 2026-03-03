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

namespace Bike2Beans.Domain.CommandsAndQueries.Coffeeshop;

public class SearchCoffeeShopByTextHandler
{
    private readonly IPlacesRestGateway _placesRest;

    public SearchCoffeeShopByTextHandler(IPlacesRestGateway placesRest)
    {
        _placesRest = placesRest;
    }

    public async Task<PagedResult<CoffeeShopDto>> Handle(
        SearchCoffeeShopByTextQuery query,
        CancellationToken ct = default
    )
    {

        var response = await _placesRest.SearchPlacesByTextAsync(query, ct);

        var result = response.Places.Select(p => new CoffeeShopDto(
            p.Id,
            p.DisplayName?.Text ?? "",
            p.FormattedAddress,
            p.Rating,
            p.UserRatingCount,
            p.Location?.Latitude,
            p.Location?.Longitude
            )).ToList();

        return new PagedResult<CoffeeShopDto>(result, response.NextPageToken ?? null);
    }
}