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

    public RouteController(GetAllRouteDetailsHandler getAll, CreateRouteDetailsHandler create)
    {
        _getAll = getAll;
        _create = create;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var routeDetails = await _getAll.Handle(new GetRouteDetailsQuery());
        if (routeDetails == null)
        {
            return NotFound();
        }
        return Ok(routeDetails);
    }
    [HttpPost]
    public async Task<IActionResult> AddNew([FromBody] CreateRouteDetailsCommand cmd, CancellationToken ct)
    {
        var created = await _create.Handle(cmd, ct);
        return Ok(created);
    }
}