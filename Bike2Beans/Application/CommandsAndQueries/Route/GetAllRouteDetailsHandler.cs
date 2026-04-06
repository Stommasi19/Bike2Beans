
using Bike2Beans.Domain.DTOs;
using Bike2Beans.Application.Interfaces;
using Bike2Beans.Models;
using MediatR;
using Bike2Beans.Application.DTOs;

namespace Bike2Beans.Application.CommandsAndQueries.Route;

public class GetAllRouteDetailsHandler : IRequestHandler<GetRouteDetailsQuery, List<RouteDetailsDto>>
{
    private readonly IRouteRepository _repo;

    public GetAllRouteDetailsHandler(IRouteRepository repo)
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
            RouteStops: d.RouteStops,
            Mileage: d.Mileage ?? 0

        )).ToList();
    }
}