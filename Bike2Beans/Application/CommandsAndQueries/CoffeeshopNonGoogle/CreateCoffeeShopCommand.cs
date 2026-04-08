
using Bike2Beans.Application.DTOs;
using MediatR;
namespace Bike2Beans.Application.CommandsAndQueries.CoffeeshopNonGoogle;

public record CreateCoffeeshopCommand(
    string PlaceId,
    string Name,
    string? Address,
    double? Lat,
    double? Lng,
    double? Rating,
    int? UserRatingsTotal
) : IRequest<CoffeeshopDto>;