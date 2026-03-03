using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Bike2Beans.Models;
using System.Reflection.Metadata.Ecma335;

namespace Bike2Beans.Domain.Repositories;

public class CoffeeShopRepository
{
    private readonly IMongoCollection<CoffeeShop> _coffeeShop;

    public CoffeeShopRepository(IOptions<MongoDBSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var db = client.GetDatabase(settings.Value.DatabaseName);
        _coffeeShop = db.GetCollection<CoffeeShop>("coffeeshop");
    }
    public async Task<List<CoffeeShop>> GetAllAsync()
    => await _coffeeShop.Find(FilterDefinition<CoffeeShop>.Empty).ToListAsync();

    public async Task<CoffeeShop> InsertAsync(CoffeeShop shop, CancellationToken ct = default)
    {
        await _coffeeShop.InsertOneAsync(shop, new InsertOneOptions(), ct);
        return shop;
    }

}