using Google.Maps.Places.V1;
using Google.Type;
using Google.Api.Gax.Grpc;
using Bike2Beans.Domain.DTOs;
using Bike2Beans.Domain.Repositories;
using MediatR;


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
            }
        };
        request.IncludedTypes.Add(DestinationTypes.IncludedTypes);

        var response = await _places.SearchNearbyAsync(request, callSettings);

        var result = response.Places.Select(p => new CoffeeshopDto(
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

