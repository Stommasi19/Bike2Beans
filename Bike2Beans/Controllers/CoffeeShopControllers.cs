using Microsoft.AspNetCore.Mvc;
using Bike2Beans.Application.CoffeeShops.Queries.GetAll;
using Bike2Beans.Application.CoffeeShops.Commands.Create;

namespace Bike2Beans.Controllers;

[ApiController]
[Route("api/coffeeshops")]
public class CoffeeShopController : ControllerBase
{
    private readonly GetAllCoffeeShopHandler _getAll;
    private readonly CreateCoffeeShopHandler _create;

    public CoffeeShopController(GetAllCoffeeShopHandler getAll, CreateCoffeeShopHandler create)
    {
        _getAll = getAll;
        _create = create;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var shops = await _getAll.Handle(new GetAllCoffeeShopQuery(), ct);
        if (shops == null)
        {
            return NotFound();
        }
        return Ok(shops);
    }


    [HttpPost]
    public async Task<IActionResult> AddNew([FromBody] CreateCoffeeShopCommand cmd, CancellationToken ct)
    {

        var created = await _create.Handle(cmd, ct);
        Console.WriteLine("POST /api/coffeeshops hit");
        return Ok(created);
    }
}