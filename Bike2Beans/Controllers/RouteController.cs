using Microsoft.AspNetCore.Mvc;
using Bike2Beans.Application.CoffeeShops.Queries.Get;
using Bike2Beans.Application.CoffeeShops.Commands.Create;
using MediatR;
namespace Bike2Beans.Controllers;

[ApiController]
[Route("api/Route")]
public class RouteController : ControllerBase
{
    private readonly ISender _sender;

    public RouteController(ISender sender) => _sender = sender;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    => Ok(await _sender.Send(new GetRouteDetailsQuery(), ct));
    [HttpGet("{routeId}")]
    public async Task<IActionResult> GetById(GetRouteDetailsByIdQuery routeId, CancellationToken ct)
    => Ok(await _sender.Send(routeId, ct));

    [HttpPost]
    public async Task<IActionResult> AddNew([FromBody] CreateRouteDetailsCommand cmd, CancellationToken ct)
    => Ok(await _sender.Send(cmd, ct));

}