using Bike2Beans.Domain.DTOs;
using MediatR;

namespace Bike2Beans.Domain.CommandsAndQueries.Route;

public record GetRouteDetailsByIdQuery(
    string Id
) : IRequest<RouteDetailsDto?>;