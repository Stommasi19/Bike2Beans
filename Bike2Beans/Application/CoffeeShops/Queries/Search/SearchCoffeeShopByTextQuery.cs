namespace Bike2Beans.Application.CoffeeShops.Queries.Search;

public record SearchCoffeeShopByTextQuery(
    string Text,
    int PageSize = 10
// string? PageToken = null
);