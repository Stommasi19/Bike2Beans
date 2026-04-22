using Bike2Beans.Application.CommandsAndQueries.Route;
using Bike2Beans.Application.Interfaces;
using Bike2Beans.Domain.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Bike2Beans.Infrastructure.Repositories;

public class RouteRepository : IRouteRepository
{
    private readonly IMongoCollection<RouteDetails> _routedetails;

    public RouteRepository(IOptions<MongoDBSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var db = client.GetDatabase(settings.Value.DatabaseName);
        _routedetails = db.GetCollection<RouteDetails>("routedetails");
    }

    public async Task<RouteDetails?> GetRouteDetailsByIdAsync(GetRouteDetailsByIdQuery query, CancellationToken ct)
    => await _routedetails.Find(BuildRouteAccessFilter(query.UserId, query.Id)).FirstOrDefaultAsync(ct);

    public async Task<List<RouteDetails>> GetAllRouteDetailsAsync(string userId, CancellationToken ct)
    => await _routedetails.Find(r => r.UserId == userId).ToListAsync(ct);

    public async Task<RouteDetails> InsertRouteDetailsAsync(RouteDetails route, CancellationToken ct = default)
    {
        await _routedetails.InsertOneAsync(route, new InsertOneOptions(), ct);
        return route;
    }

    private static FilterDefinition<RouteDetails> BuildRouteAccessFilter(string userId, string routeId)
    {
        var idFilters = new List<FilterDefinition<RouteDetails>>
        {
            Builders<RouteDetails>.Filter.Eq(r => r.Id, routeId)
        };

        if (Guid.TryParse(routeId, out var guidId))
        {
            idFilters.Add(new BsonDocument("_id", new BsonBinaryData(guidId, GuidRepresentation.Standard)));
        }

        return Builders<RouteDetails>.Filter.And(
            Builders<RouteDetails>.Filter.Eq(r => r.UserId, userId),
            Builders<RouteDetails>.Filter.Or(idFilters)
        );
    }
}
