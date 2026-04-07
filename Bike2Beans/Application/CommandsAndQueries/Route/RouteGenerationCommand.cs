using MediatR;
using Bike2Beans.Application.DTOs;
namespace Bike2Beans.Application.CommandsAndQueries.Route;

public record RouteGenerationCommand(

    List<double> StartLocation,
    List<double>? EndLocation,
    List<RouteStopDto> Stops
// RouteDto Route

) : IRequest<List<RouteOptionDto>>;