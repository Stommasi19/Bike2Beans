namespace Bike2Beans.Domain.CommandsAndQueries.CoffeeshopLocaters;


public record SearchCoffeeshopByTextQuery(
    string Text,
    int PageSize,
    string? PageToken
    );
