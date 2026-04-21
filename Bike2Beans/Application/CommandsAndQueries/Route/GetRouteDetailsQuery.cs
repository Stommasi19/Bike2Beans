using Bike2Beans.Application.DTOs;
using MediatR;

namespace Bike2Beans.Application.CommandsAndQueries.Route;

public record GetRouteDetailsQuery(string UserId) : IRequest<List<RouteDetailsDto>>;
