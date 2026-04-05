using Bike2Beans.Domain.DTOs;
using MediatR;

namespace Bike2Beans.Application.CommandsAndQueries.Route;

public record GetRouteDetailsQuery() : IRequest<List<RouteDetailsDto>>;