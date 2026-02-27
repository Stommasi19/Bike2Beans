namespace Bike2Beans.Dtos;

public record RouteOptionDto(
    int OptionIndex,
    double DistanceMeters,
    double DurationSeconds,
    string GeometryType,
    List<List<double>> Coordinates
);