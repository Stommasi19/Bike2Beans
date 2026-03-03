using Microsoft.AspNetCore.Mvc;
using MediatR;
using Bike2Beans.Domain.CommandsAndQueries.CoffeeshopNonGoogle;

namespace Bike2Beans.Api.Controllers;

[ApiController]
[Route("api/coffeeshops")]
public class CoffeeShopController : ControllerBase
{
    private readonly IMediator _mediator;

    public CoffeeShopController(IMediator sender) => _mediator = sender;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    => Ok(await _mediator.Send(new GetAllCoffeeshopQuery(), ct));



    [HttpPost]
    public async Task<IActionResult> AddNew([FromBody] CreateCoffeeshopCommand cmd, CancellationToken ct)
    => Ok(await _mediator.Send(cmd, ct));
}