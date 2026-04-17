using MediatR;
using Microsoft.AspNetCore.Mvc;
using Bike2Beans.Application.CommandsAndQueries.Route;
namespace Bike2Beans.Api.Controllers;

[ApiController]
[Route("api/Route")]
public class RouteController : ControllerBase
{
    private readonly IMediator _mediator;

    public RouteController(IMediator sender) => _mediator = sender;

    [HttpGet]
    public async Task<IActionResult> GetAllRoutesByUserId(CancellationToken ct)
    => Ok(await _mediator.Send(new GetRouteDetailsQuery(), ct));
    [HttpGet("{routeId}")]
    public async Task<IActionResult> GetRouteByRouteId([FromRoute] Guid routeId, CancellationToken ct)
    {
        var route = await _mediator.Send(new GetRouteDetailsByIdQuery(routeId), ct);
        return route is null ? NotFound() : Ok(route);
    }

    [HttpPost]
    public async Task<IActionResult> CreateNewRoute([FromBody] CreateRouteDetailsCommand cmd, CancellationToken ct)
    => Ok(await _mediator.Send(cmd, ct));

}
