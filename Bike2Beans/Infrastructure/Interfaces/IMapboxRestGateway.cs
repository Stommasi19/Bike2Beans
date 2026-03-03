using System.Threading;
using System.Threading.Tasks;
using Bike2Beans.Application.CoffeeShops.Commands.Create;
using Bike2Beans.Dtos;

namespace Bike2Beans.Infrastructure.Interfaces;

public interface IMapboxRestGateway
{
    Task<List<RouteOptionDto>> CreateRoute(
        CreateRouteCommand routeinfo,
        CancellationToken ct = default
    );
}