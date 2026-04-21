using Bike2Beans.Application.DTOs;
using Bike2Beans.Domain.Entities;
using MediatR;

namespace Bike2Beans.Application.CommandsAndQueries.Route;

public record CreateRouteDetailsCommand(
    string UserId,
    string Name,
    List<double> StartLocation,
    List<double>? EndLocation,
    List<RouteStopDto> Stops,
    double Mileage
// RouteDto Route


) : IRequest<RouteDetails>;
