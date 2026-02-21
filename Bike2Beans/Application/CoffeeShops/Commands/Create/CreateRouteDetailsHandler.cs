using Bike2Beans.Models;
using Bike2Beans.Data;



namespace Bike2Beans.Application.CoffeeShops.Commands.Create;

public class CreateRouteDetailsHandler
{
    private readonly RouteRepository _repo;

    public CreateRouteDetailsHandler(RouteRepository repo)
    {
        _repo = repo;
    }

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