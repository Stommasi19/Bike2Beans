
using Bike2Beans.Entities;
namespace Bike2Beans.Domain.DTOs;


public record RouteDetailsDto(
    string Id,
    string Name,
    List<double> StartLocation,
    List<double>? EndLocation,
    List<CoffeeShopDto> RouteStops,
    double Mileage
// route?????
);