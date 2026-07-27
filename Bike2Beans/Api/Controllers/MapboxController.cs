using Bike2Beans.Application.CommandsAndQueries.Route;
using Bike2Beans.Application.DTOs;
using Bike2Beans.Api.Configuration;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;

namespace Bike2Beans.Api.Controllers;

[ApiController]
[Authorize]
[EnableRateLimiting(ApiRateLimitPolicies.RouteGeneration)]
[Route("api/mapbox")]
public class MapboxController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ApiCostGuardOptions _apiCostGuards;

    public MapboxController(IMediator mediator, IOptions<ApiCostGuardOptions> apiCostGuards)
    {
        _mediator = mediator;
        _apiCostGuards = apiCostGuards.Value;
    }

    [HttpPost("GenerateRouteFromDetails/{routeDetailsId}")]
    public async Task<IActionResult> GenerateRouteFromDetails([FromRoute] string routeDetailsId, CancellationToken ct)
    {
        var firebaseUid = GetAuthenticatedUserId();
        if (string.IsNullOrWhiteSpace(firebaseUid)) return Unauthorized();
        if (string.IsNullOrWhiteSpace(routeDetailsId)) return BadRequest("Route id is required.");

        var routeQuery = new GetRouteDetailsByIdQuery(routeDetailsId, firebaseUid);
        var routeDetails = await _mediator.Send(routeQuery, ct);
        if (routeDetails == null) return NotFound();
        var routeStops = routeDetails.RouteStops ?? [];

        var validationError = ValidateRouteRequest(
            routeDetails.StartLocation,
            routeDetails.EndLocation ?? [],
            routeStops
        );
        if (validationError is not null)
        {
            return BadRequest(validationError);
        }

        var query = new RouteGenerationCommand(
            routeDetails.StartLocation,
            routeDetails.EndLocation ?? null,
            routeStops
        );
        var route = await _mediator.Send(query, ct);
        return Ok(route);
    }

    [HttpPost("GenerateRoute")]
    public async Task<IActionResult> GenerateRoute([FromBody] GenerateRouteRequest req,
       CancellationToken ct)
    {
        var validationError = ValidateRouteRequest(req.StartLocation, req.EndLocation, req.RouteStops);
        if (validationError is not null)
        {
            return BadRequest(validationError);
        }

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

    private string? ValidateRouteRequest(
        List<double> startLocation,
        List<double> endLocation,
        List<RouteStopDto> routeStops)
    {
        if (!IsCoordinatePair(startLocation))
        {
            return "StartLocation must include exactly two coordinates.";
        }

        if (endLocation.Count > 0 && !IsCoordinatePair(endLocation))
        {
            return "EndLocation must include exactly two coordinates when provided.";
        }

        if (routeStops.Count > _apiCostGuards.RouteStopCountMax)
        {
            return $"Routes may include up to {_apiCostGuards.RouteStopCountMax} stops.";
        }

        return null;
    }

    private static bool IsCoordinatePair(List<double> location) => location.Count == 2;

    private string? GetAuthenticatedUserId()
    {
        return User.FindFirst("user_id")?.Value ?? User.FindFirst("sub")?.Value;
    }
}
