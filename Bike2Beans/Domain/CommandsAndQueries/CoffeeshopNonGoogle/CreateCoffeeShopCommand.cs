
using Bike2Beans.Domain.DTOs;
using MediatR;
namespace Bike2Beans.Domain.CommandsAndQueries.CoffeeshopNonGoogle;

public record CreateCoffeeshopCommand(
    string Name,
    string? Address,
    double? Lat,
    double? Lng,
    double? Rating,
    int? UserRatingsTotal
) : IRequest<DTOs.CoffeeshopDto>;