
namespace Bike2Beans.Application.DTOs;


public record RouteDetailsDto(
    string Id,
    string Name,
    List<double> StartLocation,
    List<double>? EndLocation,
    List<CoffeeshopDto> RouteStops,
    double Mileage
// route?????
);