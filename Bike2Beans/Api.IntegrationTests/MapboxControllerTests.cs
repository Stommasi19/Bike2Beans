using Bike2Beans.Api.Configuration;
using Bike2Beans.Api.Controllers;
using Bike2Beans.Application.CommandsAndQueries.Route;
using Bike2Beans.Application.DTOs;
using Bike2Beans.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Api.IntegrationTests;

public class MapboxControllerTests
{
    [Fact]
    public void MapboxController_RequiresAuthorization()
    {
        Assert.NotNull(typeof(MapboxController).GetCustomAttributes(typeof(AuthorizeAttribute), inherit: true).SingleOrDefault());
    }

    [Fact]
    public async Task GenerateRoute_ReturnsBadRequest_WhenStopCountExceedsLimit()
    {
        var mediator = new RecordingMediator();
        var controller = new MapboxController(
            mediator,
            Options.Create(new ApiCostGuardOptions
            {
                RouteStopCountMax = 2
            })
        );
        var request = new MapboxController.GenerateRouteRequest
        {
            StartLocation = [47.61, -122.33],
            EndLocation = [47.62, -122.34],
            RouteStops =
            [
                CreateRouteStop("shop-1"),
                CreateRouteStop("shop-2"),
                CreateRouteStop("shop-3")
            ]
        };

        var result = await controller.GenerateRoute(request, CancellationToken.None);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Routes may include up to 2 stops.", badRequest.Value);
        Assert.Null(mediator.LastRequest);
    }

    [Fact]
    public async Task GenerateRoute_ReturnsBadRequest_WhenStartCoordinatesAreInvalid()
    {
        var mediator = new RecordingMediator();
        var controller = new MapboxController(mediator);
        var request = new MapboxController.GenerateRouteRequest
        {
            StartLocation = [47.61],
            EndLocation = [47.62, -122.34]
        };

        var result = await controller.GenerateRoute(request, CancellationToken.None);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("StartLocation must include exactly two coordinates.", badRequest.Value);
        Assert.Null(mediator.LastRequest);
    }

    [Fact]
    public async Task GenerateRoute_SendsRouteGenerationCommand_WhenRequestIsValid()
    {
        var mediator = new RecordingMediator();
        var expected = new List<RouteOptionDto>
        {
            new(Guid.NewGuid(), 0, 4500, 900, "LineString", [[-122.33, 47.61], [-122.34, 47.62]])
        };
        mediator.Register<RouteGenerationCommand, List<RouteOptionDto>>((_, _) => expected);
        var controller = new MapboxController(mediator);
        var request = new MapboxController.GenerateRouteRequest
        {
            StartLocation = [47.61, -122.33],
            EndLocation = [47.62, -122.34],
            RouteStops = [CreateRouteStop("shop-1")]
        };

        var result = await controller.GenerateRoute(request, CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Same(expected, ok.Value);
        Assert.Equal(
            new RouteGenerationCommand(request.StartLocation, request.EndLocation, request.RouteStops),
            Assert.IsType<RouteGenerationCommand>(mediator.LastRequest)
        );
    }

    private static RouteStopDto CreateRouteStop(string placeId) => new(
        Guid.NewGuid(),
        placeId,
        "Cafe",
        "123 Bean St",
        LocationTypeEnum.Coffeeshop,
        47.61,
        -122.33
    );
}
