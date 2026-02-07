using Google.Maps.Places.V1;
using Google.Protobuf.WellKnownTypes;
using Google.Type;
using Google.Api.Gax.Grpc;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Bike2Beans.Application.CoffeeShops.Queries.Get;


namespace Bike2Beans.Controllers;

[ApiController]
[Route("Api/places")]
public class PlacesController : ControllerBase
{
    private readonly SearchNearbyCoffeeShopHandler _searchNearby;
    private readonly GetCoffeeShopByIdHandler _searchById;

    public PlacesController(
        SearchNearbyCoffeeShopHandler searchNearby,
        GetCoffeeShopByIdHandler SearchById
    )
    {
        _searchNearby = searchNearby;
        _searchById = SearchById;
    }

    [HttpGet("nearby")]
    public async Task<IActionResult> Nearby(
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
    public async Task<IActionResult> GetPlaceById(
        [FromQuery] string id = "ChIJ-cPHe4xrkFQRMvbH8nZG-nc"
    )
    {
        var query = new GetCoffeeShopByIdQuery(id);
        var shop = await _searchById.Handle(query);
        return Ok(shop);
    }
}