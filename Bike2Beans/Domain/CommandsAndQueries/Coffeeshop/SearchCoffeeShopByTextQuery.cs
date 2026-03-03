using Bike2Beans.Data;
namespace Bike2Beans.Domain.CommandsAndQueries.Coffeeshop;


public record SearchCoffeeShopByTextQuery(
    string Text,
    int PageSize,
    string? PageToken,
    bool StrictTypeFiltering = true
);
