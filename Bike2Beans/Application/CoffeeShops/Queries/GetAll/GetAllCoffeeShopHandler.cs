using Bike2Beans.Data;

namespace Bike2Beans.Application.CoffeeShops.Queries.GetAll;

public class GetAllCoffeeShopHandler
{
    private readonly CoffeeShopRepository _repo;

    public GetAllCoffeeShopHandler(CoffeeShopRepository repo)
    {
        _repo = repo;
    }
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
            Rating: s.Rating
        )).ToList();
    }


}