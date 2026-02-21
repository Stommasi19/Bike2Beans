
using Bike2Beans.Models;
namespace Bike2Beans.Dtos;


public record RouteDetailsDto(
    string Id,
    string Name,
    List<RouteStop> RouteStops,
    double Mileage
// route
);