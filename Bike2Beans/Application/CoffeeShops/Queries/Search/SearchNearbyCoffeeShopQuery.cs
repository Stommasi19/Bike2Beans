

namespace Bike2Beans.Application.CoffeeShops.Queries.Search;

public record SearchNearbyCoffeeShopQuery(
    double Lat,
    double Lng,
    int RadiusMeters,
    int Max
);
