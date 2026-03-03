
using Bike2Beans.Domain.DTOs;
using Bike2Beans.Domain.Repositories;
using Bike2Beans.Models;
using MediatR;

namespace Bike2Beans.Domain.CommandsAndQueries.CoffeeshopNonGoogle;

public class CreateCoffeeShopHandler : IRequestHandler<CreateCoffeeShopCommand, DTOs.CoffeeshopDto>
{
    private readonly CoffeeShopRepository _repo;

    public CreateCoffeeShopHandler(CoffeeShopRepository repo)
    {
        _repo = repo;
    }
    public async Task<DTOs.CoffeeshopDto> Handle(CreateCoffeeShopCommand cmd, CancellationToken ct)
    {
        var shop = new Coffeeshop
        {
            Name = cmd.Name,
            Address = cmd.Address,
            Lat = cmd.Lat,
            Lng = cmd.Lng,
            Rating = cmd.Rating,
            UserRatingsTotal = cmd.UserRatingsTotal
        };

        var created = await _repo.InsertAsync(shop, ct);

        return new CoffeeshopDto(
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
