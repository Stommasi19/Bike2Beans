using Bike2Beans.Models;

namespace Bike2Beans.Application.CoffeeShops.Commands.Create;

public record CreateRouteDetailsCommand(
    string Name,
    List<RouteStop> Stops,
    double Mileage
// RouteDto Route


);