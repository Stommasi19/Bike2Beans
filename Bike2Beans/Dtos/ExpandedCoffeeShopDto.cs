namespace Bike2Beans.Dtos;

public record ExpandedCoffeeShopDto
(
    string? Id,
    string Name,
    string? Address,
    double? Rating,
    int? UserRatingsTotal,
    double? Lat,
    double? Lng
);
// subject to change