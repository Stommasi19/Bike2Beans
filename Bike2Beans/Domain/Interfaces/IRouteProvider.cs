using System.Threading;
using System.Threading.Tasks;
using Bike2Beans.Domain.DTOs;


namespace Bike2Beans.Domain.Interfaces;

public interface IRouteProvider
{
    Task<List<RouteOptionDto>> CreateRoute(
        List<double> StartLocation,
        List<double>? EndLocation,
        List<ILocation> Stops,
        CancellationToken ct = default
    );
}

