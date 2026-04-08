namespace Bike2Beans.Application.DTOs;

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