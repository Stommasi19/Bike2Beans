using Bike2Beans.Application.DTOs;
using MediatR;

namespace Bike2Beans.Application.CommandsAndQueries.Autocomplete;

public record AutocompleteQuery(
    bool Coffee,
    string? Text,
    string? SessionToken = null
) : IRequest<List<AutocompletePredictionDto>>;