namespace Bike2Beans.Application.DTOs;

public record ExpandedCoffeeshopDto
(
    Guid Id,
    string Name,
    string? Address,
    double? Rating,
    int? UserRatingsTotal,
    double? Lat,
    double? Lng
);
// subject to change