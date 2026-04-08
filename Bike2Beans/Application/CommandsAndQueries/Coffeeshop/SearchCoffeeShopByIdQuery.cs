

using Bike2Beans.Application.DTOs;
using MediatR;

namespace Bike2Beans.Application.CommandsAndQueries.CoffeeshopLocaters;


public record SearchCoffeeshopByIdQuery(
    string Id
) : IRequest<ExpandedCoffeeshopDto>;
