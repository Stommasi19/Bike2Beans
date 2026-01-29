using Bike2Beans.Dtos;
using Google.Maps.Places.V1;
using Google.Protobuf.WellKnownTypes;
using Google.Type;
using Google.Api.Gax.Grpc;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;


namespace Bike2Beans.Application.CoffeeShops.Queries.Get;



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
        request.IncludedTypes.Add("cafe");

        var response = await _places.SearchNearbyAsync(request, callSettings);

        var result = response.Places.Select(p => new CoffeeShopDto(
            Id: p.Id,
            Name: p.DisplayName?.Text ?? "",
            Address: p.FormattedAddress,
            Rating: p.Rating,
            UserRatingsTotal: p.UserRatingCount,
            Lat: p.Location?.Latitude,
            Lng: p.Location?.Longitude
        )).ToList();
        return result;
    }
}

