
using Bike2Beans.Application.CommandsAndQueries.Route;
using Bike2Beans.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;


namespace Bike2Beans.Api.Controllers;

[ApiController]
[Route("api/mapbox")]
public class MapboxController : ControllerBase
{
    private readonly IMediator _mediator;

    public MapboxController(IMediator mediator) => _mediator = mediator;

    [HttpPost("GenerateRouteFromDetails/{routeDetailsId}")]
    public async Task<IActionResult> GenerateRouteFromDetails([FromRoute] string routeDetailsId, CancellationToken ct)
    {
        var routeQuery = new GetRouteDetailsByIdQuery(routeDetailsId);
        var routeDetails = await _mediator.Send(routeQuery, ct);
        if (routeDetails == null) return NotFound();
        var query = new RouteGenerationCommand(
            routeDetails.StartLocation,
            routeDetails.EndLocation ?? null,
            routeDetails.RouteStops
        );
        var route = await _mediator.Send(query, ct);
        return Ok(route);
    }

    [HttpPost("GenerateRoute")]
    public async Task<IActionResult> GenerateRoute([FromBody] GenerateRouteRequest req,
       CancellationToken ct)
    {
        var query = new RouteGenerationCommand(
            req.StartLocation,
            req.EndLocation,
            req.RouteStops
        );
        var route = await _mediator.Send(query, ct);
        return Ok(route);
    }

    public sealed class GenerateRouteRequest
    {
        public List<double> StartLocation { get; set; } = new();
        public List<double> EndLocation { get; set; } = new();
        public List<RouteStopDto> RouteStops { get; set; } = new();
    }
}