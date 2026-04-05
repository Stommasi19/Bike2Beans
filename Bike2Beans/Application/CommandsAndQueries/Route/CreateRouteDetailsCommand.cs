using Bike2Beans.Domain.DTOs;
using Bike2Beans.Domain.Entities;
using Bike2Beans.Models;
using MediatR;

namespace Bike2Beans.Application.CommandsAndQueries.Route;

public record CreateRouteDetailsCommand(
    string? Id,
    string Name,
    List<double> StartLocation,
    List<double>? EndLocation,
    List<CoffeeshopDto> Stops,
    double Mileage
// RouteDto Route


) : IRequest<RouteDetails>;