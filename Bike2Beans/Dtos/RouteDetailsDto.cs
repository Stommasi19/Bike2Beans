
using Bike2Beans.Models;
namespace Bike2Beans.Dtos;


public record RouteDetailsDto(
    string Id,
    string Name,
    List<double> StartLocation,
    List<double>? EndLocation,
    List<CoffeeShopDto> RouteStops,
    double Mileage
// route?????
);