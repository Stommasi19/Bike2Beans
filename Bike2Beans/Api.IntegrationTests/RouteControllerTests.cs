using System.Security.Claims;
using Bike2Beans.Api.Controllers;
using Bike2Beans.Application.CommandsAndQueries.Route;
using Bike2Beans.Application.DTOs;
using Bike2Beans.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Api.IntegrationTests;

public class RouteControllerTests
{
    [Fact]
    public async Task GetAllRoutesByUserId_ReturnsUnauthorized_WhenNoAuthenticatedUserIdExists()
    {
        var mediator = new RecordingMediator();
        var controller = ControllerTestSupport.WithClaims(new RouteController(mediator));

        var result = await controller.GetAllRoutesByUserId(CancellationToken.None);

        Assert.IsType<UnauthorizedResult>(result);
        Assert.Null(mediator.LastRequest);
    }

    [Fact]
    public async Task GetRouteByRouteId_ReturnsNotFound_WhenMediatorReturnsNull()
    {
        var mediator = new RecordingMediator();
        const string routeId = "507f1f77bcf86cd799439011";
        mediator.Register<GetRouteDetailsByIdQuery, RouteDetailsDto?>((_, _) => null);
        var controller = ControllerTestSupport.WithClaims(
            new RouteController(mediator),
            new Claim("sub", "firebase-123")
        );

        var result = await controller.GetRouteByRouteId(routeId, CancellationToken.None);

        Assert.IsType<NotFoundResult>(result);
        Assert.Equal(new GetRouteDetailsByIdQuery(routeId, "firebase-123"), mediator.LastRequest);
    }

    [Fact]
    public async Task GetRouteByRouteId_ReturnsOk_WhenMediatorReturnsRoute()
    {
        var mediator = new RecordingMediator();
        const string routeId = "507f1f77bcf86cd799439011";
        var expected = new RouteDetailsDto(
            routeId,
            "Morning Loop",
            [47.61, -122.33],
            [47.65, -122.37],
            [],
            12.4
        );
        mediator.Register<GetRouteDetailsByIdQuery, RouteDetailsDto?>((_, _) => expected);
        var controller = ControllerTestSupport.WithClaims(
            new RouteController(mediator),
            new Claim("user_id", "firebase-999")
        );

        var result = await controller.GetRouteByRouteId(routeId, CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Same(expected, ok.Value);
    }

    [Fact]
    public async Task CreateNewRoute_ReturnsOk_WithCreatedRoute()
    {
        var mediator = new RecordingMediator();
        var request = new RouteController.CreateRouteRequest(
            "Coffee Loop",
            [47.61, -122.33],
            [47.64, -122.31],
            [],
            8.5
        );
        var expected = new RouteDetails(
            request.Name,
            request.StartLocation,
            request.EndLocation,
            [],
            request.Mileage
        )
        {
            Id = "507f1f77bcf86cd799439012",
            UserId = "firebase-123"
        };
        mediator.Register<CreateRouteDetailsCommand, RouteDetails>(
            (_, _) => expected
        );
        var controller = ControllerTestSupport.WithClaims(
            new RouteController(mediator),
            new Claim("sub", "firebase-123")
        );

        var result = await controller.CreateNewRoute(request, CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Same(expected, ok.Value);
        Assert.Equal(
            new CreateRouteDetailsCommand(
                "firebase-123",
                request.Name,
                request.StartLocation,
                request.EndLocation,
                request.Stops,
                request.Mileage
            ),
            Assert.IsType<CreateRouteDetailsCommand>(mediator.LastRequest)
        );
    }
}
