
namespace Bike2Beans.Application.DTOs;


public record RouteDetailsDto(
    Guid Id,
    string Name,
    List<double> StartLocation,
    List<double>? EndLocation,
    List<RouteStopDto>? RouteStops,
    double Mileage
// route?????
);