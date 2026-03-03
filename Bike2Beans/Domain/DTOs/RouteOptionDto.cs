namespace Bike2Beans.Domain.DTOs;

public record RouteOptionDto(
    int OptionIndex,
    double DistanceMeters,
    double DurationSeconds,
    string GeometryType,
List<List<double>> Coordinates
);