

using Bike2Beans.Application.Interfaces;
using Bike2Beans.Domain.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Bike2Beans.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<User> _user;

    public UserRepository(IOptions<MongoDBSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var db = client.GetDatabase(settings.Value.DatabaseName);
        _user = db.GetCollection<User>("user");
    }
    public Task<User> CreateUserAsync(User user, CancellationToken cancellationToken = default)
    {
        return _user.InsertOneAsync(user, new InsertOneOptions(), cancellationToken)
            .ContinueWith(t => user, cancellationToken);
    }

    public async Task DeleteUserAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _user.DeleteOneAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _user.Find(u => u.Email == email).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _user.Find(u => u.Id == id).FirstOrDefaultAsync(cancellationToken);
    }


    public async Task<User> PatchUserAsync(User user, CancellationToken cancellationToken = default)
    {
        var update = Builders<User>.Update
            .Set(u => u.Email, user.Email)
            .Set(u => u.FirstName, user.FirstName)
            .Set(u => u.LastName, user.LastName);

        return await _user.FindOneAndUpdateAsync(u => u.Id == user.Id, update, new FindOneAndUpdateOptions<User> { ReturnDocument = ReturnDocument.After }, cancellationToken);
    }

    public async Task<User> UpdateUserAsync(User user, CancellationToken cancellationToken = default)
    {
        return await _user.FindOneAndReplaceAsync(u => u.Id == user.Id, user, new FindOneAndReplaceOptions<User> { ReturnDocument = ReturnDocument.After }, cancellationToken);
    }
}
