using Bike2Beans.Application.DTOs;
using Bike2Beans.Application.Interfaces;
using Bike2Beans.Domain.Entities;
using MediatR;



namespace Bike2Beans.Application.CommandsAndQueries.Route;

public class RouteGenerationHandler : IRequestHandler<RouteGenerationCommand, List<RouteOptionDto>>
{
    private readonly IRouteProvider _mapbox;
    public RouteGenerationHandler(IRouteProvider mapbox)
    {
        _mapbox = mapbox;
    }
    public async Task<List<RouteOptionDto>> Handle(RouteGenerationCommand cmd, CancellationToken ct)
    {
        var response = await _mapbox.CreateRoute(cmd.StartLocation, cmd.EndLocation, cmd.Stops, ct);

        return response;
    }
}