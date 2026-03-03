using Bike2Beans.Models;
using Bike2Beans.Domain.Repositories;
using Bike2Beans.Domain.DTOs;

namespace Bike2Beans.Domain.CommandsAndQueries.Route;

public class GetRouteDetailsByIdHandler
{
    private readonly RouteRepository _repo;

    public GetRouteDetailsByIdHandler(RouteRepository repo)
    {
        _repo = repo;
    }
    public async Task<RouteDetailsDto> Handle(GetRouteDetailsByIdQuery query, CancellationToken ct)
    {
        var details = await _repo.GetRouteDetailsByIdAsync(query, ct);

        return new RouteDetailsDto(
            Id: details.Id ?? throw new InvalidOperationException("RouteDetails.Id is null"),
            Name: details.Name ?? "",
            StartLocation: details.StartLocation ?? throw new InvalidOperationException("RouteDetails.StartLocation is null"),
            EndLocation: details.EndLocation ?? null,
            RouteStops: details.Stops ?? [],
            Mileage: details.Mileage ?? 0

        );
    }
}