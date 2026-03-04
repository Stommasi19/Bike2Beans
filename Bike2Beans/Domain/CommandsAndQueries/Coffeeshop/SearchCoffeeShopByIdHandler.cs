using Google.Maps.Places.V1;
using Google.Api.Gax.Grpc;
using Bike2Beans.Domain.DTOs;
using MediatR;


namespace Bike2Beans.Domain.CommandsAndQueries.CoffeeshopLocaters;

public class SearchCoffeeshopByIdHandler : IRequestHandler<SearchCoffeeshopByIdQuery, ExpandedCoffeeshopDto>
{
    private readonly PlacesClient _places;

    public SearchCoffeeshopByIdHandler(PlacesClient places)
    {
        _places = places;
    }

    public async Task<ExpandedCoffeeshopDto> Handle(
        SearchCoffeeshopByIdQuery query,
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