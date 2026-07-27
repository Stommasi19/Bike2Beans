using Bike2Beans.Application.CommandsAndQueries.Autocomplete;
using Bike2Beans.Application.CommandsAndQueries.CoffeeshopLocaters;
using Bike2Beans.Application.DTOs;
using Bike2Beans.Api.Configuration;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;

namespace Bike2Beans.Api.Controllers;

[ApiController]
[Authorize]
[Route("Api/places")]
public class PlacesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ApiCostGuardOptions _apiCostGuards;

    public PlacesController(IMediator mediator, IOptions<ApiCostGuardOptions> apiCostGuards)
    {
        _mediator = mediator;
        _apiCostGuards = apiCostGuards.Value;
    }

    [EnableRateLimiting(ApiRateLimitPolicies.PlacesSearch)]
    [HttpGet("Nearby")]
    public async Task<IActionResult> SearchNearby(
        [FromQuery] double lat = 47.6062,
        [FromQuery] double lng = -122.3320,
        [FromQuery] int radiusMeters = 2500,
        [FromQuery] int max = 12,
        CancellationToken ct = default
    )
    {
        var normalizedRadius = Math.Clamp(radiusMeters, 1, _apiCostGuards.NearbyRadiusMetersMax);
        var normalizedMax = Math.Clamp(max, 1, _apiCostGuards.NearbyResultCountMax);
        var query = new SearchNearbyCoffeeshopQuery(lat, lng, normalizedRadius, normalizedMax);
        var shops = await _mediator.Send(query, ct);
        return Ok(shops);
    }

    [EnableRateLimiting(ApiRateLimitPolicies.PlacesSearch)]
    [HttpGet("Id")]
    public async Task<IActionResult> SearchPlaceById(
        [FromQuery] string id = "ChIJ-cPHe4xrkFQRMvbH8nZG-nc",
        CancellationToken ct = default
    )
    {
        var normalizedId = id.Trim();
        if (normalizedId.Length == 0)
        {
            return BadRequest("Place id is required.");
        }

        var query = new SearchCoffeeshopByIdQuery(normalizedId);
        var shop = await _mediator.Send(query, ct);
        return Ok(shop);
    }

    [EnableRateLimiting(ApiRateLimitPolicies.PlacesSearch)]
    [HttpGet("Text")]
    public async Task<IActionResult> SearchPlaceByText(
        [FromQuery] string Text = "URL Coffee Seattle",
        [FromQuery] int PageSize = 10,
        [FromQuery] string? PageToken = null,
        [FromQuery] bool coffeeOnly = true,
        CancellationToken ct = default
    )
    {
        var normalizedText = Text.Trim();
        if (normalizedText.Length < _apiCostGuards.TextSearchMinLength)
        {
            return Ok(EmptySearchResult());
        }

        var normalizedPageSize = Math.Clamp(PageSize, 1, _apiCostGuards.TextSearchPageSizeMax);
        var query = new SearchCoffeeshopByTextQuery(
            normalizedText,
            normalizedPageSize,
            PageToken,
            coffeeOnly
        );
        var shops = await _mediator.Send(query, ct);

        return Ok(shops);
    }

    [EnableRateLimiting(ApiRateLimitPolicies.PlacesAutocomplete)]
    [HttpGet("Autocomplete")]
    public async Task<IActionResult> AutocompleteText(
        [FromQuery] string text = "",
        CancellationToken ct = default
    )
    {
        var normalizedText = text.Trim();
        if (normalizedText.Length < _apiCostGuards.AutocompleteMinLength)
        {
            return Ok(new List<AutocompletePredictionDto>());
        }

        var query = new AutocompleteQuery(true, normalizedText);
        var response = await _mediator.Send(query, ct);

        return Ok(response);
    }

    [EnableRateLimiting(ApiRateLimitPolicies.PlacesAutocomplete)]
    [HttpGet("ExternalLocationAutocomplete")]
    public async Task<IActionResult> ExternalLocationAutocompleteText(
        [FromQuery] string text = "",
        CancellationToken ct = default
    )
    {
        var normalizedText = text.Trim();
        if (normalizedText.Length < _apiCostGuards.AutocompleteMinLength)
        {
            return Ok(new List<AutocompletePredictionDto>());
        }

        var query = new AutocompleteQuery(false, normalizedText);
        var response = await _mediator.Send(query, ct);

        return Ok(response);
    }

    private static PaginationSupportedCoffeeshopResultDto EmptySearchResult() => new()
    {
        Locations = [],
        NextPageToken = null
    };
}
