

using Bike2Beans.Application.CommandsAndQueries.Route;
using Bike2Beans.Application.DTOs;

namespace Bike2Beans.Application.Interfaces;

public interface IGateway
{
    Task<List<RouteOptionDto>> Handle(CreateRouteCommand command, CancellationToken ct);
}