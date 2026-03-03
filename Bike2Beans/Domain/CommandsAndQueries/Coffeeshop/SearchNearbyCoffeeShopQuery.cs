

namespace Bike2Beans.Domain.CommandsAndQueries.Coffeeshop;

public record SearchNearbyCoffeeShopQuery(
    double Lat,
    double Lng,
    int RadiusMeters,
    int Max
);
