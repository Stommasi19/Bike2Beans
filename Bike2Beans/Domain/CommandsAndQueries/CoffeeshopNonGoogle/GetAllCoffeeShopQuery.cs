using Bike2Beans.Dtos;
using MediatR;

namespace Bike2Beans.Domain.CommandsAndQueries.CoffeeshopNonGoogle;

public record GetAllCoffeeShopQuery() : IRequest<List<CoffeeShopDto>>;
