using Bike2Beans.Domain.Entities;

namespace Bike2Beans.Application.DTOs;

public record RouteStopDto(
    Guid Id,
    string PlaceId,
    string Name,
    string? Address,
    LocationTypeEnum LocationType,
    double Lat,
    double Lng
);

