using MediatR;
using Microsoft.AspNetCore.Mvc;
using Bike2Beans.Application.CommandsAndQueries.Route;
using Bike2Beans.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
namespace Bike2Beans.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/Route")]
public class RouteController : ControllerBase
{
    public sealed record CreateRouteRequest(
        string Name,
        List<double> StartLocation,
        List<double>? EndLocation,
        List<RouteStopDto> Stops,
        double Mileage
    );

    private readonly IMediator _mediator;

    public RouteController(IMediator sender) => _mediator = sender;

    [HttpGet]
    public async Task<IActionResult> GetAllRoutesByUserId(CancellationToken ct)
    {
        var firebaseUid = GetAuthenticatedUserId();
        if (string.IsNullOrWhiteSpace(firebaseUid)) return Unauthorized();

        return Ok(await _mediator.Send(new GetRouteDetailsQuery(firebaseUid), ct));
    }

    [HttpGet("{routeId}")]
    public async Task<IActionResult> GetRouteByRouteId([FromRoute] string routeId, CancellationToken ct)
    {
        var firebaseUid = GetAuthenticatedUserId();
        if (string.IsNullOrWhiteSpace(firebaseUid)) return Unauthorized();
        if (string.IsNullOrWhiteSpace(routeId)) return BadRequest("Route id is required.");

        var route = await _mediator.Send(new GetRouteDetailsByIdQuery(routeId, firebaseUid), ct);
        return route is null ? NotFound() : Ok(route);
    }

    [HttpPost]
    public async Task<IActionResult> CreateNewRoute([FromBody] CreateRouteRequest request, CancellationToken ct)
    {
        var firebaseUid = GetAuthenticatedUserId();
        if (string.IsNullOrWhiteSpace(firebaseUid)) return Unauthorized();

        var command = new CreateRouteDetailsCommand(
            firebaseUid,
            request.Name,
            request.StartLocation,
            request.EndLocation,
            request.Stops,
            request.Mileage
        );

        return Ok(await _mediator.Send(command, ct));
    }

    private string? GetAuthenticatedUserId()
    {
        return User.FindFirst("user_id")?.Value ?? User.FindFirst("sub")?.Value;
    }
}
