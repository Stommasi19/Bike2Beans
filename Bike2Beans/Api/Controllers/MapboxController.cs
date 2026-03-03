
using Bike2Beans.Domain.CommandsAndQueries.Route;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;


namespace Bike2Beans.Api.Controllers;

[ApiController]
[Route("Api/Mapbox")]
public class MapboxController : ControllerBase
{
    private readonly CreateRouteHandler _createRoute;
    private readonly GetRouteDetailsByIdHandler _getById;

    public MapboxController(
        CreateRouteHandler createRoute,
        GetRouteDetailsByIdHandler getById
    )
    {
        _createRoute = createRoute;
        _getById = getById;
    }

    [HttpPost("GenerateRouteFromDetails/{routeDetailsId}")]
    public async Task<IActionResult> GenerateRouteFromDetails([FromRoute] string routeDetailsId, CancellationToken ct)
    {
        var routeQuery = new GetRouteDetailsByIdQuery(routeDetailsId);
        var routeDetails = await _getById.Handle(routeQuery, ct);
        if (routeDetails == null) return NotFound();
        var query = new CreateRouteCommand(
            routeDetails.Id,
            routeDetails.Name,
            routeDetails.StartLocation,
            routeDetails.EndLocation ?? null,
            routeDetails.RouteStops,
            routeDetails.Mileage
        );
        var route = await _createRoute.Handle(query, ct);
        return Ok(route);
    }

}