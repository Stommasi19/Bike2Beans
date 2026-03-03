
namespace Bike2Beans.Domain.DTOs;


public record RouteDetailsDto(
    string Id,
    string Name,
    List<double> StartLocation,
    List<double>? EndLocation,
    List<CoffeeshopDto> RouteStops,
    double Mileage
// route?????
);