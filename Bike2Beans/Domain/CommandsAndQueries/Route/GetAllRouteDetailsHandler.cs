
using Bike2Beans.Domain.DTOs;
using Bike2Beans.Domain.Repositories;
using Bike2Beans.Models;

namespace Bike2Beans.Domain.CommandsAndQueries.Route;

public class GetAllRouteDetailsHandler
{
    private readonly RouteRepository _repo;

    public GetAllRouteDetailsHandler(RouteRepository repo)
    {
        _repo = repo;
    }
    public async Task<List<RouteDetailsDto>> Handle(GetRouteDetailsQuery query, CancellationToken ct)
    {
        var details = await _repo.GetAllRouteDetailsAsync(ct);

        return details.Select(d => new RouteDetailsDto(
            Id: d.Id ?? throw new InvalidOperationException("RouteDetails.Id is null"),
            Name: d.Name ?? "",
            StartLocation: d.StartLocation ?? throw new InvalidOperationException("RouteDetails.StartLocation is null"),
            EndLocation: d.EndLocation ?? null,
            RouteStops: d.Stops,
            Mileage: d.Mileage ?? 0

        )).ToList();
    }
}