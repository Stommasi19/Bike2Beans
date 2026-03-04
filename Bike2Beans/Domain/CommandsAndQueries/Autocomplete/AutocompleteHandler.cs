using Google.Maps.Places.V1;
using Google.Protobuf.WellKnownTypes;
using Google.Type;
using Google.Api.Gax.Grpc;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Bike2Beans.Infrastructure;
using Bike2Beans.Domain.DTOs;
using Bike2Beans.Domain.Repositories;
using MediatR;

namespace Bike2Beans.Domain.CommandsAndQueries.Autocomplete;

public class AutocompleteHandler : IRequestHandler<AutocompleteQuery, List<AutocompletePredictionDto>>
{
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

        if (!query.external)
        {
            request.IncludedPrimaryTypes.AddRange(DestinationTypes.IncludedTypes);
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