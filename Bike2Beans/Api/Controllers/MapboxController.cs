
using Bike2Beans.Domain.CommandsAndQueries.Route;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;


namespace Bike2Beans.Api.Controllers;

[ApiController]
[Route("Api/Mapbox")]
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
        var query = new CreateRouteCommand(
            routeDetails.Id,
            routeDetails.Name,
            routeDetails.StartLocation,
            routeDetails.EndLocation ?? null,
            routeDetails.RouteStops,
            routeDetails.Mileage
        );
        var route = await _mediator.Send(query, ct);
        return Ok(route);
    }

}