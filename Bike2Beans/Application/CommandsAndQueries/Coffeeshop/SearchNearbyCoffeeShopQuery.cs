using Bike2Beans.Application.DTOs;
using MediatR;

namespace Bike2Beans.Application.CommandsAndQueries.CoffeeshopLocaters;

public record SearchNearbyCoffeeshopQuery(
    double Lat,
    double Lng,
    int RadiusMeters,
    int Max
) : IRequest<List<CoffeeshopDto>>;
