using Microsoft.AspNetCore.Mvc;
using Bike2Beans.Application.CoffeeShops.Queries.Get;
using Bike2Beans.Application.CoffeeShops.Commands.Create;
using MediatR;
namespace Bike2Beans.Api.Controllers;

[ApiController]
[Route("api/Route")]
public class RouteController : ControllerBase
{
    private readonly IMeditator _mediator;

    public RouteController(IMeditator sender) => _mediator = sender;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    => Ok(await _mediator.Send(new GetRouteDetailsQuery(), ct));
    [HttpGet("{routeId}")]
    public async Task<IActionResult> GetById(GetRouteDetailsByIdQuery routeId, CancellationToken ct)
    => Ok(await _mediator.Send(routeId, ct));

    [HttpPost]
    public async Task<IActionResult> AddNew([FromBody] CreateRouteDetailsCommand cmd, CancellationToken ct)
    => Ok(await _mediator.Send(cmd, ct));

}