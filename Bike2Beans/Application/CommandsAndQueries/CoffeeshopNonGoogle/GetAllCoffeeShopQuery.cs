using Bike2Beans.Application.DTOs;
using MediatR;

namespace Bike2Beans.Application.CommandsAndQueries.CoffeeshopNonGoogle;

public record GetAllCoffeeshopQuery() : IRequest<List<CoffeeshopDto>>;

