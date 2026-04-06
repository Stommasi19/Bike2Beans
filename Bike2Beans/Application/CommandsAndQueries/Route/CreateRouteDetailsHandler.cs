using Bike2Beans.Models;
using MediatR;
using Bike2Beans.Domain.Entities;
using Bike2Beans.Application.Interfaces;

namespace Bike2Beans.Application.CommandsAndQueries.Route;

public class CreateRouteDetailsHandler : IRequestHandler<CreateRouteDetailsCommand, RouteDetails>
{
    private readonly IRouteRepository _repo;

    public CreateRouteDetailsHandler(IRouteRepository repo) => _repo = repo;

    public async Task<RouteDetails> Handle(CreateRouteDetailsCommand cmd, CancellationToken ct)
    {
        var RouteDetails = new RouteDetails
        {
            Name = cmd.Name,
            StartLocation = cmd.StartLocation,
            EndLocation = cmd.EndLocation ?? null,
            RouteStops = cmd.Stops,
            Mileage = cmd.Mileage
        };
        return await _repo.InsertRouteDetailsAsync(RouteDetails, ct);
    }
}