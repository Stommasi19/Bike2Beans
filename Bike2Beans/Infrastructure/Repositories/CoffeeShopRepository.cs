using Microsoft.Extensions.Options;
using MongoDB.Driver;

using Bike2Beans.Application.Interfaces;
using Bike2Beans.Domain.Entities;

namespace Bike2Beans.Infrastructure.Repositories;

public class CoffeeShopRepository : ICoffeeshopRepository
{
    private readonly IMongoCollection<Coffeeshop> _coffeeShop;

    public CoffeeShopRepository(IOptions<MongoDBSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var db = client.GetDatabase(settings.Value.DatabaseName);
        _coffeeShop = db.GetCollection<Coffeeshop>("coffeeshop");
    }
    public async Task<List<Coffeeshop>> GetAllAsync()
    => await _coffeeShop.Find(FilterDefinition<Coffeeshop>.Empty).ToListAsync();

    public async Task<Coffeeshop> InsertAsync(Coffeeshop shop, CancellationToken ct = default)
    {
        await _coffeeShop.InsertOneAsync(shop, new InsertOneOptions(), ct);
        return shop;
    }

}