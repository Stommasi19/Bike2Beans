using Bike2Beans.Application.CommandsAndQueries.Route;
using Bike2Beans.Application.DTOs;
using Bike2Beans.Domain.Entities;

namespace Application.Tests;

public class RouteGenerationHandlerTests
{
    [Fact]
    public async Task Handle_ForwardsLocationsStopsAndCancellationToken()
    {
        var provider = new RecordingRouteProvider();
        var expectedRoute = new RouteOptionDto(
            Guid.NewGuid(),
            0,
            3200,
            780,
            "LineString",
            [[-122.3, 47.6], [-122.2, 47.61]]
        );
        provider.Result = [expectedRoute];

        var stops = new List<RouteStopDto>
        {
            new(
                Guid.NewGuid(),
                "shop-1",
                "Midpoint Cafe",
                "45 Cycle Ave",
                LocationTypeEnum.Coffeeshop,
                47.605,
                -122.31
            )
        };
        var command = new RouteGenerationCommand([47.60, -122.33], [47.62, -122.35], stops);
        var handler = new RouteGenerationHandler(provider);
        using var cts = new CancellationTokenSource();

        var result = await handler.Handle(command, cts.Token);

        Assert.Same(provider.Result, result);
        Assert.Equal(command.StartLocation, provider.CapturedStartLocation);
        Assert.Equal(command.EndLocation, provider.CapturedEndLocation);
        Assert.Equal(command.Stops, provider.CapturedStops);
        Assert.Equal(cts.Token, provider.CapturedCancellationToken);
    }
}
