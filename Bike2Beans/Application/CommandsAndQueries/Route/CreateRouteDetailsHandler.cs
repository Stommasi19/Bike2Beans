using MediatR;
using Bike2Beans.Domain.Entities;
using Bike2Beans.Application.Interfaces;
using Bike2Beans.Application.DTOs;

namespace Bike2Beans.Application.CommandsAndQueries.Route;

public class CreateRouteDetailsHandler : IRequestHandler<CreateRouteDetailsCommand, RouteDetails>
{
    private readonly IRouteRepository _repo;
    private readonly IMapper<RouteStop, RouteStopDto> _mapper;

    public CreateRouteDetailsHandler(IRouteRepository repo, IMapper<RouteStop, RouteStopDto> mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<RouteDetails> Handle(CreateRouteDetailsCommand cmd, CancellationToken ct)
    {
        var RouteDetails = new RouteDetails
        (
            cmd.Name,
            cmd.StartLocation,
            cmd.EndLocation ?? null,
            cmd.Stops.Select(_mapper.ToEntity).ToList(),
            cmd.Mileage
        )
        {
            UserId = cmd.UserId
        };
        return await _repo.InsertRouteDetailsAsync(RouteDetails, ct);
    }
}
