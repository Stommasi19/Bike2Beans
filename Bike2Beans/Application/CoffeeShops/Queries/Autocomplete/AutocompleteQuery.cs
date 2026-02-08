namespace Bike2Beans.Application.CoffeeShops.Queries.Autocomplete;

public record AutocompleteQuery(
    string? Text,
    string? SessionToken = null
);