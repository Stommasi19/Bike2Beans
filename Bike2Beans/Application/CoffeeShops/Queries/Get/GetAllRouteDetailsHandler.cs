using Bike2Beans.Dtos;
using Bike2Beans.Data;
using Bike2Beans.Models;

namespace Bike2Beans.Application.CoffeeShops.Queries.Get;

public class GetAllRouteDetailsHandler
{
    private readonly RouteRepository _repo;

    public GetAllRouteDetailsHandler(RouteRepository repo)
    {
        _repo = repo;
    }
    public async Task<List<RouteDetailsDto>> Handle(GetRouteDetailsQuery query, CancellationToken ct)
    {
        var details = await _repo.GetAllRouteDetailsAsync();

        return details.Select(d => new RouteDetailsDto(
            Id: d.Id ?? throw new InvalidOperationException("RouteDetails.Id is null"),
            Name: d.Name ?? "",
            RouteStops: d.Stops ?? new List<RouteStop>(),
            Mileage: d.Mileage ?? 0

        )).ToList();
    }
}