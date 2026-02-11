namespace Bike2Beans.Application.CoffeeShops.Queries.Search;

public record SearchCoffeeShopByTextQuery(
    string Text,
    int PageSize,
    string? PageToken,
    string IncludedType = "cafe",
    bool StrictTypeFiltering = true
);
