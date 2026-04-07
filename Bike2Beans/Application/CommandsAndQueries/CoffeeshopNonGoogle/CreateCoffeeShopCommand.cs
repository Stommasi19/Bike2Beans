
using MediatR;
namespace Bike2Beans.Application.CommandsAndQueries.CoffeeshopNonGoogle;

public record CreateCoffeeshopCommand(
    string Name,
    string? Address,
    double? Lat,
    double? Lng,
    double? Rating,
    int? UserRatingsTotal
) : IRequest<DTOs.CoffeeshopDto>;