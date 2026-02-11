using Google.Maps.Places.V1;
using Google.Protobuf.WellKnownTypes;
using Google.Type;
using Google.Api.Gax.Grpc;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Bike2Beans.Application.CoffeeShops.Queries.Get;
using Bike2Beans.Application.CoffeeShops.Queries.Search;
using Bike2Beans.Application.CoffeeShops.Queries.Autocomplete;



namespace Bike2Beans.Controllers;

[ApiController]
[Route("Api/places")]
public class PlacesController : ControllerBase
{
    private readonly SearchNearbyCoffeeShopHandler _searchNearby;
    private readonly SearchCoffeeShopByIdHandler _searchById;
    private readonly SearchCoffeeShopByTextHandler _searchByText;
    private readonly AutocompleteHandler _autocompleteSearch;

    public PlacesController(
        SearchNearbyCoffeeShopHandler searchNearby,
        SearchCoffeeShopByIdHandler SearchById,
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
        var query = new SearchNearbyCoffeeShopQuery(lat, lng, radiusMeters, max);
        var shops = await _searchNearby.Handle(query);
        return Ok(shops);
    }
    [HttpGet("Id")]
    public async Task<IActionResult> SearchPlaceById(
        [FromQuery] string id = "ChIJ-cPHe4xrkFQRMvbH8nZG-nc"
    )
    {
        var query = new SearchCoffeeShopByIdQuery(id);
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
        var query = new SearchCoffeeShopByTextQuery(
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