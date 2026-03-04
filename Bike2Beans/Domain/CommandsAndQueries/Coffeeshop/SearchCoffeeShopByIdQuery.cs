

using Bike2Beans.Domain.DTOs;
using MediatR;

namespace Bike2Beans.Domain.CommandsAndQueries.CoffeeshopLocaters;


public record SearchCoffeeshopByIdQuery(
    string Id
) : IRequest<ExpandedCoffeeshopDto>;
