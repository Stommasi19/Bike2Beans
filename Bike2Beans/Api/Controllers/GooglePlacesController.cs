
using Bike2Beans.Application.CommandsAndQueries.Autocomplete;
using Bike2Beans.Application.CommandsAndQueries.CoffeeshopLocaters;
using MediatR;
using Microsoft.AspNetCore.Mvc;




namespace Bike2Beans.Api.Controllers;

[ApiController]
[Route("Api/places")]
public class PlacesController : ControllerBase
{
    private readonly IMediator _mediator;

    public PlacesController(IMediator mediator) => _mediator = mediator;

    [HttpGet("Nearby")]
    public async Task<IActionResult> SearchNearby(
        [FromQuery] double lat = 42.4370,
        [FromQuery] double lng = -71.5056,
        [FromQuery] int radiusMeters = 5000,
        [FromQuery] int max = 20,
        CancellationToken ct = default
    )
    {
        var query = new SearchNearbyCoffeeshopQuery(lat, lng, radiusMeters, max);
        var shops = await _mediator.Send(query);
        return Ok(shops);
    }
    [HttpGet("Id")]
    public async Task<IActionResult> SearchPlaceById(
        [FromQuery] string id = "ChIJ-cPHe4xrkFQRMvbH8nZG-nc"
    )
    {
        var query = new SearchCoffeeshopByIdQuery(id);
        var shop = await _mediator.Send(query);
        return Ok(shop);
    }

    [HttpGet("Text")]
    public async Task<IActionResult> SearchPlaceByText(
        [FromQuery] string Text = "URL Coffee Seattle",
        [FromQuery] int PageSize = 10,
        [FromQuery] string? PageToken = null,
        [FromQuery] bool coffeeOnly = true
    )
    {
        var query = new SearchCoffeeshopByTextQuery(
            Text,
            PageSize,
            PageToken,
            coffeeOnly
            );
        var shops = await _mediator.Send(query);



        return Ok(shops);
    }

    [HttpGet("Autocomplete")]
    public async Task<IActionResult> AutocompleteText(
        [FromQuery] string text = ""
    )
    {
        var query = new AutocompleteQuery(true, text);
        var response = await _mediator.Send(query);

        return Ok(response);
    }
    [HttpGet("ExternalLocationAutocomplete")]
    public async Task<IActionResult> ExternalLocationAutocompleteText(
        [FromQuery] string text = ""
    )
    {
        var query = new AutocompleteQuery(false, text);
        var response = await _mediator.Send(query);

        return Ok(response);
    }



}
