using Bike2Beans.Domain.DTOs;
using MediatR;

namespace Bike2Beans.Domain.CommandsAndQueries.CoffeeshopLocaters;


public record SearchCoffeeshopByTextQuery(
    string Text,
    int PageSize,
    string? PageToken,
    bool CoffeeOnly = true
    ) : IRequest<PaginationSupportedCoffeeshopResultDto>;
