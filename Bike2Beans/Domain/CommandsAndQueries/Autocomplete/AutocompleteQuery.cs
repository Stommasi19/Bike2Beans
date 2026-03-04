using Bike2Beans.Domain.DTOs;
using MediatR;

namespace Bike2Beans.Domain.CommandsAndQueries.Autocomplete;

public record AutocompleteQuery(
    bool external,
    string? Text,
    string? SessionToken = null
) : IRequest<List<AutocompletePredictionDto>>;