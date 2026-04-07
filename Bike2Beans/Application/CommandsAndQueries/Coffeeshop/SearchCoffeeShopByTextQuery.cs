using Bike2Beans.Application.DTOs;
using MediatR;

namespace Bike2Beans.Application.CommandsAndQueries.CoffeeshopLocaters;


public record SearchCoffeeshopByTextQuery(
    string Text,
    int PageSize,
    string? PageToken,
    bool CoffeeOnly = true
    ) : IRequest<PaginationSupportedCoffeeshopResultDto>;
