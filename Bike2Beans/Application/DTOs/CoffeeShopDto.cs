namespace Bike2Beans.Application.DTOs;

public record CoffeeshopDto
(
    Guid? Id,
    string? PlaceId,
    string Name,
    string? Address,
    double? Lat,
    double? Lng,
    double? Rating,
    int? UserRatingsTotal

);
