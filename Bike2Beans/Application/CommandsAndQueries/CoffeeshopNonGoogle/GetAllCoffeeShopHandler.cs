

using Bike2Beans.Application.DTOs;
using Bike2Beans.Application.Interfaces;
using MediatR;

namespace Bike2Beans.Application.CommandsAndQueries.CoffeeshopNonGoogle;

public class GetAllCoffeeshopHandler : IRequestHandler<GetAllCoffeeshopQuery, List<CoffeeshopDto>>
{
    private readonly ICoffeeshopRepository _repo;

    public GetAllCoffeeshopHandler(ICoffeeshopRepository repo) => _repo = repo;

    public async Task<List<CoffeeshopDto>> Handle(GetAllCoffeeshopQuery query, CancellationToken ct = default)
    {
        var shops = await _repo.GetAllAsync();

        //mapping
        return shops.Select(s => new CoffeeshopDto(
            Id: s.Id,
            PlaceId: s.PlaceId,
            Name: s.Name,
            Address: s.Address,
            Lat: s.Lat,
            Lng: s.Lng,
            Rating: s.Rating,
            UserRatingsTotal: s.UserRatingsTotal
        )).ToList();
    }


}