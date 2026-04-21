
using Bike2Beans.Application.Interfaces;
using MediatR;
using Bike2Beans.Application.DTOs;
using Bike2Beans.Domain.Entities;

namespace Bike2Beans.Application.CommandsAndQueries.Route;

public class GetAllRouteDetailsHandler : IRequestHandler<GetRouteDetailsQuery, List<RouteDetailsDto>>
{
    private readonly IRouteRepository _repo;
    private readonly IMapper<RouteStop, RouteStopDto> _mapper;

    public GetAllRouteDetailsHandler(IRouteRepository repo, IMapper<RouteStop, RouteStopDto> mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }
    public async Task<List<RouteDetailsDto>> Handle(GetRouteDetailsQuery query, CancellationToken ct)
    {
        var details = await _repo.GetAllRouteDetailsAsync(query.UserId, ct);

        return details.Select(d => new RouteDetailsDto(
            Id: d.Id,
            Name: d.Name ?? "",
            StartLocation: d.StartLocation ?? throw new InvalidOperationException("RouteDetails.StartLocation is null"),
            EndLocation: d.EndLocation ?? null,
            RouteStops: d.RouteStops?.Select(_mapper.ToDto).ToList() ?? new List<RouteStopDto>(),
            Mileage: d.Mileage ?? 0

        )).ToList();
    }
}
