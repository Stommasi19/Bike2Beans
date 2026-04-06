using Google.Maps.Places.V1;
using Google.Protobuf.WellKnownTypes;
using Google.Type;
using Google.Api.Gax.Grpc;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Bike2Beans.Application.DTOs;
using MediatR;

namespace Bike2Beans.Application.CommandsAndQueries.Autocomplete;

public class AutocompleteHandler : IRequestHandler<AutocompleteQuery, List<AutocompletePredictionDto>>
{
    private static IReadOnlyList<string> IncludedTypes =
        new List<string>
        {
            "cafe",
            "bakery"
        };
    private readonly PlacesClient _places;

    public AutocompleteHandler(PlacesClient places)
    {
        _places = places;
    }

    public async Task<List<AutocompletePredictionDto>> Handle(
        AutocompleteQuery query,
        CancellationToken ct = default
    )
    {
        if (query.Text == "")
        {
            return new List<AutocompletePredictionDto>();
        }

        AutocompletePlacesRequest request = new AutocompletePlacesRequest
        {
            Input = query.Text,
            // LocationBias = new AutocompletePlacesRequest.Types.LocationBias(),
            // LocationRestriction = new AutocompletePlacesRequest.Types.LocationRestriction(),
            // IncludedRegionCodes = { "", },
            // LanguageCode = "",
            // RegionCode = "",
            // Origin = new LatLng(),
            // InputOffset = 0,
            // IncludeQueryPredictions = false,
            // SessionToken = "",``
            // IncludePureServiceAreaBusinesses = false,
        };

        if (query.Coffee == true)
        {
            request.IncludedPrimaryTypes.AddRange(IncludedTypes);
        }

        var response = await _places.AutocompletePlacesAsync(request);

        var predictions = new List<AutocompletePredictionDto>();

        foreach (var suggestion in response.Suggestions)
        {
            predictions.Add(new AutocompletePredictionDto(suggestion.PlacePrediction.Text.Text));
        }
        return predictions;

    }


}