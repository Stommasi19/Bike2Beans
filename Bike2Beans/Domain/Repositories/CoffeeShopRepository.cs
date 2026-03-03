using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Bike2Beans.Models;
using Bike2Beans.Models.Entities;

namespace Bike2Beans.Domain.Repositories;

public class CoffeeShopRepository
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