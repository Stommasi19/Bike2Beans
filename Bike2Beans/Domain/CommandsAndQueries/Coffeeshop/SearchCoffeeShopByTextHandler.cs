using Google.Maps.Places.V1;
using Google.Protobuf.WellKnownTypes;
using Google.Type;
using Google.Api.Gax.Grpc;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Bike2Beans.Infrastructure;
using Bike2Beans.Domain.Gateways;
using Bike2Beans.Domain.DTOs;

namespace Bike2Beans.Domain.CommandsAndQueries.CoffeeshopLocaters;

public class SearchCoffeeShopByTextHandler
{
    private readonly GooglePlacesRestGateway _placesRest;

    public SearchCoffeeShopByTextHandler(GooglePlacesRestGateway placesRest) => _placesRest = placesRest;


    public async Task<PaginationSupportedCoffeeshopResultDto> Handle(
        SearchCoffeeshopByTextQuery query,
        CancellationToken ct = default
    )
    {

        var response = await _placesRest.SearchPlacesByTextAsync(query.Text, query.PageSize, query.PageToken, ct);

        var result = response.Locations.Select(p => new CoffeeshopDto(
            p.Id,
            p.Name ?? "",
            p.Address,
            p.Rating,
            p.UserRatingsTotal,
            p.Lat,
            p.Lng
            )).ToList();

        return new PaginationSupportedCoffeeshopResultDto()
        {
            Locations = result,
            NextPageToken = response.NextPageToken ?? null
        };
    }
}