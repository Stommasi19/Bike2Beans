

namespace Bike2Beans.Domain.CommandsAndQueries.CoffeeshopLocaters;

public record SearchNearbyCoffeeshopQuery(
    double Lat,
    double Lng,
    int RadiusMeters,
    int Max
);
