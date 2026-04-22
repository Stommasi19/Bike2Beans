using Bike2Beans.Api.Configuration;
using Bike2Beans.Api.Controllers;
using Bike2Beans.Application.CommandsAndQueries.Autocomplete;
using Bike2Beans.Application.CommandsAndQueries.CoffeeshopLocaters;
using Bike2Beans.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Api.IntegrationTests;

public class PlacesControllerTests
{
    [Fact]
    public void PlacesController_RequiresAuthorization()
    {
        Assert.NotNull(typeof(PlacesController).GetCustomAttributes(typeof(AuthorizeAttribute), inherit: true).SingleOrDefault());
    }

    [Fact]
    public async Task SearchNearby_ClampsRadiusAndMax_BeforeSendingMediatorRequest()
    {
        var mediator = new RecordingMediator();
        mediator.Register<SearchNearbyCoffeeshopQuery, List<CoffeeshopDto>>((_, _) => []);
        var controller = new PlacesController(
            mediator,
            Options.Create(new ApiCostGuardOptions
            {
                NearbyRadiusMetersMax = 1500,
                NearbyResultCountMax = 5
            })
        );

        var result = await controller.SearchNearby(47.61, -122.33, radiusMeters: 5000, max: 25);

        Assert.IsType<OkObjectResult>(result);
        Assert.Equal(
            new SearchNearbyCoffeeshopQuery(47.61, -122.33, 1500, 5),
            Assert.IsType<SearchNearbyCoffeeshopQuery>(mediator.LastRequest)
        );
    }

    [Fact]
    public async Task SearchPlaceByText_ReturnsEmptyResult_WhenQueryIsTooShort()
    {
        var mediator = new RecordingMediator();
        var controller = new PlacesController(
            mediator,
            Options.Create(new ApiCostGuardOptions
            {
                TextSearchMinLength = 3
            })
        );

        var result = await controller.SearchPlaceByText("go", PageSize: 10, PageToken: null, coffeeOnly: true);

        var ok = Assert.IsType<OkObjectResult>(result);
        var payload = Assert.IsType<PaginationSupportedCoffeeshopResultDto>(ok.Value);
        Assert.Empty(payload.Locations);
        Assert.Null(payload.NextPageToken);
        Assert.Null(mediator.LastRequest);
    }

    [Fact]
    public async Task SearchPlaceByText_ClampsPageSize_BeforeSendingMediatorRequest()
    {
        var mediator = new RecordingMediator();
        mediator.Register<SearchCoffeeshopByTextQuery, PaginationSupportedCoffeeshopResultDto>(
            (_, _) => new PaginationSupportedCoffeeshopResultDto
            {
                Locations = [],
                NextPageToken = null
            }
        );
        var controller = new PlacesController(
            mediator,
            Options.Create(new ApiCostGuardOptions
            {
                TextSearchPageSizeMax = 4
            })
        );

        var result = await controller.SearchPlaceByText("Seattle coffee", PageSize: 20, PageToken: "next", coffeeOnly: false);

        Assert.IsType<OkObjectResult>(result);
        Assert.Equal(
            new SearchCoffeeshopByTextQuery("Seattle coffee", 4, "next", false),
            Assert.IsType<SearchCoffeeshopByTextQuery>(mediator.LastRequest)
        );
    }

    [Fact]
    public async Task AutocompleteText_ReturnsEmptyList_WhenQueryIsTooShort()
    {
        var mediator = new RecordingMediator();
        var controller = new PlacesController(
            mediator,
            Options.Create(new ApiCostGuardOptions
            {
                AutocompleteMinLength = 3
            })
        );

        var result = await controller.AutocompleteText("ab");

        var ok = Assert.IsType<OkObjectResult>(result);
        var payload = Assert.IsType<List<AutocompletePredictionDto>>(ok.Value);
        Assert.Empty(payload);
        Assert.Null(mediator.LastRequest);
    }
}
