using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Bike2Beans.Models;
using System.Reflection.Metadata.Ecma335;

namespace Bike2Beans.Data;

public class RouteRepository
{
    private readonly IMongoCollection<RouteDetails> _routedetails;

    public RouteRepository(IOptions<MongoDBSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var db = client.GetDatabase(settings.Value.DatabaseName);
        _routedetails = db.GetCollection<RouteDetails>("routedetails");
    }

    public async Task<List<RouteDetails>> GetAllRouteDetailsAsync(CancellationToken ct)
    => await _routedetails.Find(FilterDefinition<RouteDetails>.Empty).ToListAsync(ct);
    public async Task<RouteDetails> InsertRouteDetailsAsync(RouteDetails route, CancellationToken ct = default)
    {
        await _routedetails.InsertOneAsync(route, new InsertOneOptions(), ct);
        return route;
    }

}