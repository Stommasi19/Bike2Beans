using Bike2Beans.Models;
using Bike2Beans.Infrastructure;
using MediatR;
using Bike2Beans.Domain.DTOs;
using Bike2Beans.Infrastructure.Gateways;


namespace Bike2Beans.Domain.CommandsAndQueries.Route;

public class CreateRouteHandler : IRequestHandler<CreateRouteCommand, List<RouteOptionDto>>
{
    private readonly MapboxRestGateway _mapbox;

    public CreateRouteHandler(MapboxRestGateway mapbox)
    {
        _mapbox = mapbox;
    }
    public async Task<List<RouteOptionDto>> Handle(CreateRouteCommand cmd, CancellationToken ct)
    {

        var response = await _mapbox.CreateRoute(cmd.StartLocation, cmd.EndLocation, cmd.Stops, ct);

        return response;
    }
}