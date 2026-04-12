

using Bike2Beans.Application.Interfaces;
using Bike2Beans.Domain.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Bike2Beans.Infrastructure.Repositories;

public class UserBootstrapRepository : IUserBootstrapRepository
{
    private readonly IMongoCollection<User> _collection;
    public UserBootstrapRepository(IOptions<MongoDBSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var db = client.GetDatabase(settings.Value.DatabaseName);
        _collection = db.GetCollection<User>("user");
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _collection.Find(x => x.Email == email).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<User?> GetByAuthIdAsync(Guid authId, CancellationToken cancellationToken = default)
    {
        return await _collection.Find(x => x.AuthId == authId).FirstOrDefaultAsync(cancellationToken);
    }


    private static string GetCollectionName<T>()
    {
        var entityName = typeof(T).Name;
        return char.ToLowerInvariant(entityName[0]) + entityName[1..] + "s";
    }

}
