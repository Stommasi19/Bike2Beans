namespace Bike2Beans.Application.DTOs;

public record RouteOptionDto(
    Guid Id,
    int OptionIndex,
    double DistanceMeters,
    double DurationSeconds,
    string GeometryType,
List<List<double>> Coordinates
);