using Bike2Beans.Domain.DTOs;
using MediatR;

namespace Bike2Beans.Domain.CommandsAndQueries.Route;

public record GetRouteDetailsQuery() : IRequest<List<RouteDetailsDto>>;