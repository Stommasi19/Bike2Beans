using Bike2Beans.Models;
using Bike2Beans.Dtos;
using MediatR;
namespace Bike2Beans.Application.CoffeeShops.Commands.Create;

public record CreateRouteCommand(
    string Id,
    string Name,
    List<double> StartLocation,
    List<double>? EndLocation,
    List<CoffeeShopDto> Stops,
    double Mileage
// RouteDto Route

) : IRequest<List<RouteOptionDto>>;