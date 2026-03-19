using Bike2Beans.Domain.Gateways;
using Bike2Beans.Domain.DTOs;
using MediatR;
using Bike2Beans.Domain.Interfaces;

namespace Bike2Beans.Domain.CommandsAndQueries.CoffeeshopLocaters;

public class SearchCoffeeshopByTextHandler : IRequestHandler<SearchCoffeeshopByTextQuery, PaginationSupportedCoffeeshopResultDto>
{
    private readonly ILocationProvider _placesRest;

    public SearchCoffeeshopByTextHandler(ILocationProvider placesRest) => _placesRest = placesRest;


    public async Task<PaginationSupportedCoffeeshopResultDto> Handle(
        SearchCoffeeshopByTextQuery query,
        CancellationToken ct = default
    )
    {

        var response = await _placesRest.SearchPlacesByTextAsync(
            query.Text,
            query.PageSize,
            query.PageToken,
            query.CoffeeOnly,
            ct
        );

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
