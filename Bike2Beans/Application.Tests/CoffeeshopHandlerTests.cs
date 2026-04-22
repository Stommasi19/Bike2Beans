using Bike2Beans.Application.CommandsAndQueries.CoffeeshopNonGoogle;
using Bike2Beans.Domain.Entities;
using Bike2Beans.Domain.Mapper;

namespace Application.Tests;

public class CoffeeshopHandlerTests
{
    [Fact]
    public async Task CreateHandler_InsertsShop_AndMapsReturnedEntity()
    {
        var repository = new RecordingCoffeeshopRepository();
        var storedShop = new Coffeeshop(
            "place-123",
            "Daily Grind",
            "123 Bean St",
            47.61,
            -122.33,
            4.7,
            118
        )
        {
            Id = Guid.NewGuid()
        };
        repository.InsertAsyncHandler = (_, _) => Task.FromResult(storedShop);

        var handler = new CreateCoffeeshopHandler(repository, new CoffeeshopMapper());
        var command = new CreateCoffeeshopCommand(
            "place-123",
            "Daily Grind",
            "123 Bean St",
            47.61,
            -122.33,
            4.7,
            118
        );
        using var cts = new CancellationTokenSource();

        var result = await handler.Handle(command, cts.Token);

        Assert.NotNull(repository.InsertedShop);
        Assert.Equal(command.PlaceId, repository.InsertedShop.PlaceId);
        Assert.Equal(command.Name, repository.InsertedShop.Name);
        Assert.Equal(command.Address, repository.InsertedShop.Address);
        Assert.Equal(command.Lat, repository.InsertedShop.Lat);
        Assert.Equal(command.Lng, repository.InsertedShop.Lng);
        Assert.Equal(command.Rating, repository.InsertedShop.Rating);
        Assert.Equal(command.UserRatingsTotal, repository.InsertedShop.UserRatingsTotal);
        Assert.Equal(cts.Token, repository.InsertCancellationToken);

        Assert.Equal(storedShop.Id, result.Id);
        Assert.Equal(storedShop.PlaceId, result.PlaceId);
        Assert.Equal(storedShop.Name, result.Name);
        Assert.Equal(storedShop.Address, result.Address);
    }

    [Fact]
    public async Task GetAllHandler_MapsEveryRepositoryShop()
    {
        var firstShop = new Coffeeshop("alpha", "Alpha Roast", "1 Main St", 1.1, 2.2, 4.4, 11)
        {
            Id = Guid.NewGuid()
        };
        var secondShop = new Coffeeshop("beta", "Beta Beans", "2 Main St", 3.3, 4.4, 4.8, 22)
        {
            Id = Guid.NewGuid()
        };
        var repository = new RecordingCoffeeshopRepository
        {
            GetAllResult = [firstShop, secondShop]
        };

        var handler = new GetAllCoffeeshopHandler(repository);

        var result = await handler.Handle(new GetAllCoffeeshopQuery());

        Assert.Collection(
            result,
            dto =>
            {
                Assert.Equal(firstShop.Id, dto.Id);
                Assert.Equal(firstShop.Name, dto.Name);
                Assert.Equal(firstShop.Address, dto.Address);
            },
            dto =>
            {
                Assert.Equal(secondShop.Id, dto.Id);
                Assert.Equal(secondShop.Name, dto.Name);
                Assert.Equal(secondShop.Address, dto.Address);
            }
        );
    }
}
