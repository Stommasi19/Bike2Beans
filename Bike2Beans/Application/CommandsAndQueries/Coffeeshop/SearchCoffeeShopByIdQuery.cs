

using Bike2Beans.Domain.DTOs;
using MediatR;

namespace Bike2Beans.Application.CommandsAndQueries.CoffeeshopLocaters;


public record SearchCoffeeshopByIdQuery(
    string Id
) : IRequest<ExpandedCoffeeshopDto>;
