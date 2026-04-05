using Bike2Beans.Models;
using Bike2Beans.Domain;
using MediatR;
using Bike2Beans.Domain.DTOs;
using Bike2Beans.Domain.Interfaces;
namespace Bike2Beans.Application.CommandsAndQueries.Route;

public record CreateRouteCommand(

    List<double> StartLocation,
    List<double>? EndLocation,
    List<CoffeeshopDto> Stops
// RouteDto Route

) : IRequest<List<RouteOptionDto>>;