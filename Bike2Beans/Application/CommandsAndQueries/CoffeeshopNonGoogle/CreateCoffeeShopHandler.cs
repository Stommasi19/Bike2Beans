
using Bike2Beans.Application.DTOs;
using Bike2Beans.Application.Interfaces;
using Bike2Beans.Domain.Entities;
using MediatR;

namespace Bike2Beans.Application.CommandsAndQueries.CoffeeshopNonGoogle;

public class CreateCoffeeshopHandler : IRequestHandler<CreateCoffeeshopCommand, CoffeeshopDto>
{
    private readonly ICoffeeshopRepository _repo;
    private readonly IMapper<Coffeeshop, CoffeeshopDto> _mapper;

    public CreateCoffeeshopHandler(ICoffeeshopRepository repo, IMapper<Coffeeshop, CoffeeshopDto> mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }
    public async Task<CoffeeshopDto> Handle(CreateCoffeeshopCommand cmd, CancellationToken ct)
    {
        var shop = new Coffeeshop
        (
            cmd.PlaceId,
            cmd.Name,
            cmd.Address,
            cmd.Lat,
            cmd.Lng,
            cmd.Rating,
            cmd.UserRatingsTotal
        );

        var created = await _repo.InsertAsync(shop, ct);

        return _mapper.ToDto(created);
    }
}
