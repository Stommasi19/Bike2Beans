
using Bike2Beans.Application.DTOs;
using Bike2Beans.Application.Interfaces;
using Bike2Beans.Models.Entities;
using MediatR;

namespace Bike2Beans.Application.CommandsAndQueries.CoffeeshopNonGoogle;

public class CreateCoffeeshopHandler : IRequestHandler<CreateCoffeeshopCommand, DTOs.CoffeeshopDto>
{
    private readonly ICoffeeshopRepository _repo;

    public CreateCoffeeshopHandler(ICoffeeshopRepository repo)
    {
        _repo = repo;
    }
    public async Task<CoffeeshopDto> Handle(CreateCoffeeshopCommand cmd, CancellationToken ct)
    {
        var shop = new Coffeeshop()
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
