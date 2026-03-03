namespace Bike2Beans.Domain.CommandsAndQueries.Autocomplete;

public record AutocompleteQuery(
    string? Text,
    string? SessionToken = null
);