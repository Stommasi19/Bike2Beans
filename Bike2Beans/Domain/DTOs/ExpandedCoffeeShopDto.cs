namespace Bike2Beans.Domain.DTOs;

public record ExpandedCoffeeshopDto
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