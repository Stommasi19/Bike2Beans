using Microsoft.AspNetCore.Mvc;
using Bike2Beans.Application.CoffeeShops.Queries.Get;
using Bike2Beans.Application.CoffeeShops.Commands.Create;
using MediatR;

namespace Bike2Beans.Controllers;

[ApiController]
[Route("api/coffeeshops")]
public class CoffeeShopController : ControllerBase
{
    private readonly IMeditator _mediator;

    public CoffeeShopController(IMeditator sender) => _mediator = sender;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    => Ok(await _mediator.Send(new GetAllCoffeeShopQuery(), ct));



    [HttpPost]
    public async Task<IActionResult> AddNew([FromBody] CreateCoffeeShopCommand cmd, CancellationToken ct)
    => Ok(await _mediator.Send(cmd, ct));
}