using Bike2Beans.Application.DTOs;
using MediatR;
using Bike2Beans.Application.Interfaces;

namespace Bike2Beans.Application.CommandsAndQueries.Route;

public class GetRouteDetailsByIdHandler : IRequestHandler<GetRouteDetailsByIdQuery, RouteDetailsDto?>
{
    private readonly IRouteRepository _repo;

    public GetRouteDetailsByIdHandler(IRouteRepository repo)
    {
        _repo = repo;
    }
    public async Task<RouteDetailsDto?> Handle(GetRouteDetailsByIdQuery query, CancellationToken ct)
    {
        var details = await _repo.GetRouteDetailsByIdAsync(query, ct);

        if (details == null) return null;

        return new RouteDetailsDto(
            Id: details.Id ?? throw new InvalidOperationException("RouteDetails.Id is null"),
            Name: details.Name ?? "",
            StartLocation: details.StartLocation ?? throw new InvalidOperationException("RouteDetails.StartLocation is null"),
            EndLocation: details.EndLocation ?? null,
            RouteStops: details.RouteStops,
            Mileage: details.Mileage ?? 0

        );
    }
}
