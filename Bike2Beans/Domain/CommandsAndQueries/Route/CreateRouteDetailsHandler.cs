using Bike2Beans.Models;
using MediatR;
using Bike2Beans.Domain.Entities;
using Bike2Beans.Domain.Repositories;

namespace Bike2Beans.Domain.CommandsAndQueries.Route;

public class CreateRouteDetailsHandler : IRequestHandler<CreateRouteDetailsCommand, RouteDetails>
{
    private readonly RouteRepository _repo;

    public CreateRouteDetailsHandler(RouteRepository repo) => _repo = repo;

    public async Task<RouteDetails> Handle(CreateRouteDetailsCommand cmd, CancellationToken ct)
    {
        var RouteDetails = new RouteDetails
        {
            Name = cmd.Name,
            Stops = cmd.Stops,
            Mileage = cmd.Mileage
        };
        return await _repo.InsertRouteDetailsAsync(RouteDetails, ct);
    }
}