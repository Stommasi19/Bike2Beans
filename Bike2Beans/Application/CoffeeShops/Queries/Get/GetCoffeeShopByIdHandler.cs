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


namespace Bike2Beans.Application.CoffeeShops.Queries.Get;

public class GetCoffeeShopByIdHandler
{
    private readonly PlacesClient _places;

    public GetCoffeeShopByIdHandler(PlacesClient places)
    {
        _places = places;
    }

    public async Task<ExpandedCoffeeShopDto> Handle(
        GetCoffeeShopByIdQuery query,
        CancellationToken ct = default
        )
    {
        var fieldMask = "id,displayName,formattedAddress,rating,userRatingCount,location,photos";

        var callSettings = CallSettings
        .FromHeader("X-Goog-FieldMask", fieldMask);

        var request = new GetPlaceRequest
        {
            Name = $"places/{query.id}"
        };

        var response = await _places.GetPlaceAsync(request, callSettings);

        // TODO expandedcoffeeShopDto will be changing
        return new ExpandedCoffeeShopDto(
            response.Id,
            response.DisplayName?.Text ?? "",
            response.FormattedAddress ?? "",
            response.Rating,
            response.UserRatingCount,
            response.Location.Latitude,
            response.Location.Longitude
            );

    }
}