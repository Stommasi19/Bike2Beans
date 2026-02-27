using Microsoft.AspNetCore.Mvc;
using Bike2Beans.Application.CoffeeShops.Queries.Get;
using Bike2Beans.Application.CoffeeShops.Commands.Create;

namespace Bike2Beans.Controllers;

[ApiController]
[Route("api/Route")]
public class RouteController : ControllerBase
{
    private readonly GetAllRouteDetailsHandler _getAll;
    private readonly CreateRouteDetailsHandler _create;
    private readonly GetRouteDetailsByIdHandler _getById;

    public RouteController(GetAllRouteDetailsHandler getAll, CreateRouteDetailsHandler create, GetRouteDetailsByIdHandler getById)
    {
        _getAll = getAll;
        _create = create;
        _getById = getById;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var routeDetails = await _getAll.Handle(new GetRouteDetailsQuery(), ct);
        if (routeDetails == null)
        {
            return NotFound();
        }
        return Ok(routeDetails);
    }
    [HttpGet("{routeId}")]
    public async Task<IActionResult> GetById(GetRouteDetailsByIdQuery routeId, CancellationToken ct)
    {
        var routeDetails = await _getById.Handle(routeId, ct);
        if (routeDetails == null) return NotFound();

        return Ok(routeDetails);
    }

    [HttpPost]
    public async Task<IActionResult> AddNew([FromBody] CreateRouteDetailsCommand cmd, CancellationToken ct)
    {
        var created = await _create.Handle(cmd, ct);
        return Ok(created);
    }

}