using Bike2Beans.Models;
using Bike2Beans.Domain;
using MediatR;
using Bike2Beans.Domain.DTOs;
namespace Bike2Beans.Domain.CommandsAndQueries.Route;

public record CreateRouteCommand(
    string Id,
    string Name,
    List<double> StartLocation,
    List<double>? EndLocation,
    List<CoffeeshopDto> Stops
// RouteDto Route

) : IRequest<List<RouteOptionDto>>;