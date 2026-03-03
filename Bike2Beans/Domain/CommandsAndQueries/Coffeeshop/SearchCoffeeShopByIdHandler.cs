using Google.Maps.Places.V1;
using Google.Protobuf.WellKnownTypes;
using Google.Type;
using Google.Api.Gax.Grpc;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Bike2Beans.Infrastructure;
using Bike2Beans.Domain.DTOs;


namespace Bike2Beans.Domain.CommandsAndQueries.Coffeeshop;

public class SearchCoffeeShopByIdHandler
{
    private readonly PlacesClient _places;

    public SearchCoffeeShopByIdHandler(PlacesClient places)
    {
        _places = places;
    }

    public async Task<ExpandedCoffeeshopDto> Handle(
        SearchCoffeeShopByIdQuery query,
        CancellationToken ct = default
        )
    {
        var fieldMask = "id,displayName,formattedAddress,rating,userRatingCount,location,photos";

        var callSettings = CallSettings
        .FromHeader("X-Goog-FieldMask", fieldMask);

        var request = new GetPlaceRequest
        {
            Name = $"places/{query.Id}"
        };

        var response = await _places.GetPlaceAsync(request, callSettings);

        // TODO expandedcoffeeShopDto will be changing
        return new ExpandedCoffeeshopDto(
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