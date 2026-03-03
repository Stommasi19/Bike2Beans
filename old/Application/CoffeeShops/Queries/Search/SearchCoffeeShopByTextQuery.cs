using Bike2Beans.Data;
namespace Bike2Beans.Application.CoffeeShops.Queries.Search;


public record SearchCoffeeShopByTextQuery(
    string Text,
    int PageSize,
    string? PageToken,
    bool StrictTypeFiltering = true
);
