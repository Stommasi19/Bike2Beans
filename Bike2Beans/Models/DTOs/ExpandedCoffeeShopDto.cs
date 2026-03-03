namespace Bike2Beans.Models.Dtos;

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