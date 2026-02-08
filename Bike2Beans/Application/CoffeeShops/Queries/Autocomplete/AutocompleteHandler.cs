using Bike2Beans.Dtos;
using Google.Maps.Places.V1;
using Google.Protobuf.WellKnownTypes;
using Google.Type;
using Google.Api.Gax.Grpc;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Bike2Beans.Infrastructure;

namespace Bike2Beans.Application.CoffeeShops.Queries.Autocomplete;

public class AutocompleteHandler
{
    private readonly PlacesClient _places;

    public AutocompleteHandler(PlacesClient places)
    {
        _places = places;
    }

    // public async Task<AutocompletePredictionDto> Handle(
    //     AutocompleteQuery query,
    //     CancellationToken ct = default
    // )
    // {
    //     var too = query;
    // }
}