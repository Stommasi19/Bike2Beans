using Bike2Beans.Data;
using Bike2Beans.Dtos;
using MediatR;

namespace Bike2Beans.Domain.CommandsAndQueries.CoffeeshopNonGoogle;

public class GetAllCoffeeShopHandler : IRequestHandler<GetAllCoffeeShopQuery, List<CoffeeShopDto>>
{
    private readonly CoffeeShopRepository _repo;

    public GetAllCoffeeShopHandler(CoffeeShopRepository repo) => _repo = repo;

    public async Task<List<CoffeeShopDto>> Handle(GetAllCoffeeShopQuery query, CancellationToken ct = default)
    {
        var shops = await _repo.GetAllAsync();

        //mapping
        return shops.Select(s => new CoffeeShopDto(
            Id: s.Id,
            Name: s.Name,
            Address: s.Address,
            Lat: s.Lat,
            Lng: s.Lng,
            Rating: s.Rating,
            UserRatingsTotal: s.UserRatingsTotal
            )).ToList();
    }


}