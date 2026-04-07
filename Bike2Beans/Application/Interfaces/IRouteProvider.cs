using System.Threading;
using System.Threading.Tasks;
using Bike2Beans.Application.DTOs;


namespace Bike2Beans.Application.Interfaces;

public interface IRouteProvider
{
    Task<List<RouteOptionDto>> CreateRoute(
        List<double> StartLocation,
        List<double>? EndLocation,
        List<RouteStopDto> Stops,
        CancellationToken ct = default
    );
}

