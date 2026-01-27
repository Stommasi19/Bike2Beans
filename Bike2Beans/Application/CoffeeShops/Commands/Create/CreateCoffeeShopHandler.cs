using Bike2Beans.Models;
using Bike2Beans.Data;

namespace Bike2Beans.Application.CoffeeShops.Commands.Create;

public class CreateCoffeeShopHandler
{
    private readonly CoffeeShopRepository _repo;

    public CreateCoffeeShopHandler(CoffeeShopRepository repo)
    {
        _repo = repo;
    }
    public async Task<CoffeeShop> Handle(CreateCoffeeShopCommand cmd, CancellationToken ct = default)
    {
        var shop = new CoffeeShop
        {
            Name = cmd.Name,
            Address = cmd.Address,
            Lat = cmd.Lat,
            Lng = cmd.Lng,
            Rating = cmd.Rating
        };
        return await _repo.InsertAsync(shop, ct);
    }
}