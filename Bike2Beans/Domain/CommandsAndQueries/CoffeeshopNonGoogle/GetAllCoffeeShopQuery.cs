using MediatR;

namespace Bike2Beans.Domain.CommandsAndQueries.CoffeeshopNonGoogle;

public record GetAllCoffeeShopQuery() : IRequest<List<CoffeeshopDto>>;

public class CoffeeshopDto
{
}