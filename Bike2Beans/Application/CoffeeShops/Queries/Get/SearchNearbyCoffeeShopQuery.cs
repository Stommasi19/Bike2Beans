

namespace Bike2Beans.Application.CoffeeShops.Queries.Get;

public record SearchNearbyCoffeeShopQuery(
    double Lat,
    double Lng,
    int RadiusMeters,
    int Max
);
