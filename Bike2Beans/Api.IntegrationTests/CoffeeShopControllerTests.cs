using Bike2Beans.Api.Controllers;
using Bike2Beans.Application.CommandsAndQueries.CoffeeshopNonGoogle;
using Bike2Beans.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Api.IntegrationTests;

public class CoffeeShopControllerTests
{
    [Fact]
    public async Task GetAll_ReturnsOk_WithMediatorResults()
    {
        var mediator = new RecordingMediator();
        var expected = new List<CoffeeshopDto>
        {
            new(Guid.NewGuid(), "place-1", "Daily Grind", "123 Bean St", 47.61, -122.33, 4.5, 100)
        };
        mediator.Register<GetAllCoffeeshopQuery, List<CoffeeshopDto>>((_, _) => expected);
        var controller = new CoffeeShopController(mediator);
        using var cts = new CancellationTokenSource();

        var result = await controller.GetAll(cts.Token);

        var ok = Assert.IsType<OkObjectResult>(result);
        var payload = Assert.IsType<List<CoffeeshopDto>>(ok.Value);
        Assert.Same(expected, payload);
        Assert.IsType<GetAllCoffeeshopQuery>(mediator.LastRequest);
        Assert.Equal(cts.Token, mediator.LastCancellationToken);
    }

    [Fact]
    public async Task AddNew_ReturnsOk_WithCreatedShop()
    {
        var mediator = new RecordingMediator();
        var command = new CreateCoffeeshopCommand(
            "place-2",
            "Corner Cafe",
            "45 Market St",
            47.62,
            -122.35,
            4.8,
            210
        );
        var expected = new CoffeeshopDto(
            Guid.NewGuid(),
            command.PlaceId,
            command.Name,
            command.Address,
            command.Lat,
            command.Lng,
            command.Rating,
            command.UserRatingsTotal
        );
        mediator.Register<CreateCoffeeshopCommand, CoffeeshopDto>((_, _) => expected);
        var controller = new CoffeeShopController(mediator);

        var result = await controller.AddNew(command, CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Same(expected, ok.Value);
        Assert.Equal(command, Assert.IsType<CreateCoffeeshopCommand>(mediator.LastRequest));
    }
}
