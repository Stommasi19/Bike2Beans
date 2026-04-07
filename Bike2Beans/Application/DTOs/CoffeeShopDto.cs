namespace Bike2Beans.Application.DTOs;

public record CoffeeshopDto
(
    string? Id,
    string? PlaceId,
    string Name,
    string? Address,
    double? Rating,
    int? UserRatingsTotal,
    double? Lat,
    double? Lng
);
