using Bike2Beans.Models;
using Bike2Beans.Data;
using Bike2Beans.Dtos;
using Bike2Beans.Infrastructure;
using Bike2Beans.Application.Common;
using MediatR;


namespace Bike2Beans.Application.CoffeeShops.Commands.Create;

public class CreateRouteHandler : IRequestHandler<CreateRouteCommand, List<RouteOptionDto>>
{
    private readonly MapboxRestGateway _mapbox;

    public CreateRouteHandler(MapboxRestGateway mapbox)
    {
        _mapbox = mapbox;
    }
    public async Task<List<RouteOptionDto>> Handle(CreateRouteCommand cmd, CancellationToken ct)
    {
        // var RouteDetails = new RouteDetails 
        // {
        //     Id = cmd.Id,
        //     Name = cmd.Name,
        //     StartLocation = cmd.StartLocation,
        //     EndLocation = cmd.EndLocation != null ? cmd.EndLocation : null,
        //     Stops = cmd.Stops,
        //     Mileage = cmd.Mileage
        // };
        var response = await _mapbox.CreateRoute(cmd, ct);

        return response;
    }
}