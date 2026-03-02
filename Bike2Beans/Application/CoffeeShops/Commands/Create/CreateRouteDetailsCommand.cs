using Bike2Beans.Models;
using Bike2Beans.Dtos;
using MediatR;

namespace Bike2Beans.Application.CoffeeShops.Commands.Create;

public record CreateRouteDetailsCommand(
    string Id,
    string Name,
    List<CoffeeShopDto> Stops,
    double Mileage
// RouteDto Route


) : IRequest<RouteDetails>;