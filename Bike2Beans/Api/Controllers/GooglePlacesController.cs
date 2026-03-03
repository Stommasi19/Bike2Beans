
using Bike2Beans.Domain.CommandsAndQueries.Autocomplete;
using Bike2Beans.Domain.CommandsAndQueries.CoffeeshopLocaters;
using Microsoft.AspNetCore.Mvc;




namespace Bike2Beans.Api.Controllers;

[ApiController]
[Route("Api/places")]
public class PlacesController : ControllerBase
{
    private readonly SearchNearbyCoffeeShopHandler _searchNearby;
    private readonly SearchCoffeeshopByIdHandler _searchById;
    private readonly SearchCoffeeShopByTextHandler _searchByText;
    private readonly AutocompleteHandler _autocompleteSearch;

    public PlacesController(
        SearchNearbyCoffeeShopHandler searchNearby,
        SearchCoffeeshopByIdHandler SearchById,
        SearchCoffeeShopByTextHandler SearchByText,
        AutocompleteHandler AutocompleteSearch
    )
    {
        _searchNearby = searchNearby;
        _searchById = SearchById;
        _searchByText = SearchByText;
        _autocompleteSearch = AutocompleteSearch;
    }

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
        var shops = await _searchNearby.Handle(query);
        return Ok(shops);
    }
    [HttpGet("Id")]
    public async Task<IActionResult> SearchPlaceById(
        [FromQuery] string id = "ChIJ-cPHe4xrkFQRMvbH8nZG-nc"
    )
    {
        var query = new SearchCoffeeshopByIdQuery(id);
        var shop = await _searchById.Handle(query);
        return Ok(shop);
    }

    [HttpGet("Text")]
    public async Task<IActionResult> SearchPlaceByText(
        [FromQuery] string Text = "URL Coffee Seattle",
        [FromQuery] int PageSize = 10,
        [FromQuery] string? PageToken = null
    )
    {
        var query = new SearchCoffeeshopByTextQuery(
            Text,
            PageSize,
            PageToken
            );
        var shops = await _searchByText.Handle(query);



        return Ok(shops);
    }

    [HttpGet("Autocomplete")]
    public async Task<IActionResult> AutocompleteText(
        [FromQuery] string text = ""
    )
    {
        var query = new AutocompleteQuery(text);
        var response = await _autocompleteSearch.Handle(query);

        return Ok(response);
    }
}