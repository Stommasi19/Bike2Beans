using Google.Maps.Places.V1;
using Google.Type;
using Google.Api.Gax.Grpc;

using MediatR;
using Bike2Beans.Application.DTOs;
using Bike2Beans.Application.CommandsAndQueries.Types;


namespace Bike2Beans.Application.CommandsAndQueries.CoffeeshopLocaters;



public class SearchNearbyCoffeeshopHandler : IRequestHandler<SearchNearbyCoffeeshopQuery, List<CoffeeshopDto>>
{
    private readonly PlacesClient _places;

    public SearchNearbyCoffeeshopHandler(PlacesClient places)
    {
        _places = places;
    }

    public async Task<List<CoffeeshopDto>> Handle(
        SearchNearbyCoffeeshopQuery query,
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

            },
            RankPreference = SearchNearbyRequest.Types.RankPreference.Distance


        };
        request.IncludedTypes.Add("cafe");
        var response = await _places.SearchNearbyAsync(request, callSettings);

        var result = response.Places.Select(p => new CoffeeshopDto(
            null,
            PlaceId: p.Id,
            Name: p.DisplayName?.Text ?? "",
            Address: p.FormattedAddress,
            Lat: p.Location.Latitude,
            Lng: p.Location.Longitude,
            Rating: p.Rating,
            UserRatingsTotal: p.UserRatingCount

            )).ToList();
        return result;
    }
}

