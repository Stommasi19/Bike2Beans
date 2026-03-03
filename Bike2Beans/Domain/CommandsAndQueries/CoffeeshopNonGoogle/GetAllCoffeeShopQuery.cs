using Bike2Beans.Domain.DTOs;
using MediatR;

namespace Bike2Beans.Domain.CommandsAndQueries.CoffeeshopNonGoogle;

public record GetAllCoffeeshopQuery() : IRequest<List<CoffeeshopDto>>;

