
using MediatR;
using Bike2Beans.Application.Interfaces;
using Bike2Beans.Application.DTOs;

namespace Bike2Beans.Application.CommandsAndQueries.CoffeeshopLocaters;

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
