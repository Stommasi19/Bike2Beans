
using Bike2Beans.Dtos;
using MediatR;
namespace Bike2Beans.Domain.CommandsAndQueries.CoffeeshopNonGoogle;

public record CreateCoffeeShopCommand(
    string Name,
    string? Address,
    double? Lat,
    double? Lng,
    double? Rating,
    int? UserRatingsTotal
) : IRequest<CoffeeShopDto>;