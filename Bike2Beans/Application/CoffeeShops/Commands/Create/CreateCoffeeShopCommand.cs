
using Bike2Beans.Dtos;
using MediatR;
namespace Bike2Beans.Application.CoffeeShops.Commands.Create;

public record CreateCoffeeShopCommand(
    string Name,
    string? Address,
    double? Lat,
    double? Lng,
    double? Rating,
    int? UserRatingsTotal
) : IRequest<CoffeeShopDto>;