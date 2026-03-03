using Bike2Beans.Data;
using Bike2Beans.Dtos;
using Bike2Beans.Models;
using MediatR;

namespace Bike2Beans.Application.CoffeeShops.Commands.Create;

public class CreateCoffeeShopHandler : IRequestHandler<CreateCoffeeShopCommand, CoffeeShopDto>
{
    private readonly CoffeeShopRepository _repo;

    public CreateCoffeeShopHandler(CoffeeShopRepository repo)
    {
        _repo = repo;
    }
    public async Task<CoffeeShopDto> Handle(CreateCoffeeShopCommand cmd, CancellationToken ct)
    {
        var shop = new CoffeeShop
        {
            Name = cmd.Name,
            Address = cmd.Address,
            Lat = cmd.Lat,
            Lng = cmd.Lng,
            Rating = cmd.Rating,
            UserRatingsTotal = cmd.UserRatingsTotal
        };

        var created = await _repo.InsertAsync(shop, ct);

        return new CoffeeShopDto(
            Id: created.Id,
            Name: created.Name,
            Address: created.Address,
            Rating: created.Rating,
            UserRatingsTotal: created.UserRatingsTotal,
            Lat: created.Lat,
            Lng: created.Lng
        );
    }
}
