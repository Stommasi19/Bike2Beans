namespace Bike2Beans.Models.Dtos;

public record RouteOptionDto(
    int OptionIndex,
    double DistanceMeters,
    double DurationSeconds,
    string GeometryType,
    List<List<double>> Coordinates
);