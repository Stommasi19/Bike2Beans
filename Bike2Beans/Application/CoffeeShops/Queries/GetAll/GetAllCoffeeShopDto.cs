namespace Bike2Beans.Application.CoffeeShops.Queries.GetAll;

public record CoffeeShopDto
(
    string? Id,
    string Name,
    string? Address,
    double? Lat,
    double? Lng,
    double? Rating
);
