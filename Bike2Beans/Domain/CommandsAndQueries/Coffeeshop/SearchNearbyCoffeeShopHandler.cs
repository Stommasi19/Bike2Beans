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
using Bike2Beans.Data;


namespace Bike2Beans.Domain.CommandsAndQueries.Coffeeshop;



public class SearchNearbyCoffeeShopHandler
{
    private readonly PlacesClient _places;

    public SearchNearbyCoffeeShopHandler(PlacesClient places)
    {
        _places = places;
    }

    public async Task<List<CoffeeShopDto>> Handle(
        SearchNearbyCoffeeShopQuery query,
        CancellationToken ct = default
    )
    {
        var fieldMask = "places.id,places.displayName,places.formattedAddress,places.location,places.rating,places.userRatingCount";

        var callSettings = CallSettings
        .FromHeader("X-Goog-FieldMask", fieldMask);



        var request = new SearchNearbyRequest
        {
            MaxResultCount = query.Max,
            LocationRestriction = new SearchNearbyRequest.Types.LocationRestriction
            {
                Circle = new Circle
                {
                    Center = new LatLng { Latitude = query.Lat, Longitude = query.Lng },
                    Radius = query.RadiusMeters
                }
            }
        };
        request.IncludedTypes.Add(DestinationTypes.IncludedTypes);

        var response = await _places.SearchNearbyAsync(request, callSettings);

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
    }
}

