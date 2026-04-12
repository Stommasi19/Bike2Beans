using Bike2Beans.Application.DTOs;
using MediatR;
using Bike2Beans.Application.Interfaces;
using Bike2Beans.Domain.Entities;

namespace Bike2Beans.Application.CommandsAndQueries.Route;

public class GetRouteDetailsByIdHandler : IRequestHandler<GetRouteDetailsByIdQuery, RouteDetailsDto?>
{
    private readonly IRouteRepository _repo;
    private readonly IMapper<RouteStop, RouteStopDto> _mapper;

    public GetRouteDetailsByIdHandler(IRouteRepository repo, IMapper<RouteStop, RouteStopDto> mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }
    public async Task<RouteDetailsDto?> Handle(GetRouteDetailsByIdQuery query, CancellationToken ct)
    {
        var details = await _repo.GetRouteDetailsByIdAsync(query, ct);

        if (details == null) return null;
        var routeStops = details.RouteStops?.Select(_mapper.ToDto).ToList() ?? new List<RouteStopDto>();
        return new RouteDetailsDto(
            Id: details.Id,
            Name: details.Name ?? "",
            StartLocation: details.StartLocation ?? throw new InvalidOperationException("RouteDetails.StartLocation is null"),
            EndLocation: details.EndLocation ?? null,
            RouteStops: routeStops,
            Mileage: details.Mileage ?? 0
        );
    }
}
