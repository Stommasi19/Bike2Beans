

using Bike2Beans.Application.CommandsAndQueries.Route;
using Bike2Beans.Domain.Entities;

namespace Bike2Beans.Application.Interfaces;

public interface IRouteRepository
{
    Task<RouteDetails> GetRouteDetailsByIdAsync(GetRouteDetailsByIdQuery query, CancellationToken ct);
    Task<RouteDetails> InsertRouteDetailsAsync(RouteDetails route, CancellationToken ct);
    Task<List<RouteDetails>> GetAllRouteDetailsAsync(CancellationToken ct);
}
