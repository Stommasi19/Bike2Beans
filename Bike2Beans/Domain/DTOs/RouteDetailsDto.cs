
using Bike2Beans.Entities;
namespace Bike2Beans.Models.Dtos;


public record RouteDetailsDto(
    string Id,
    string Name,
    List<double> StartLocation,
    List<double>? EndLocation,
    List<CoffeeShopDto> RouteStops,
    double Mileage
// route?????
);