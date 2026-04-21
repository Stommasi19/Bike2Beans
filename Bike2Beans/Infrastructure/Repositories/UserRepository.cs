

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

    public async Task<User> CreateUserAsync(User user, CancellationToken cancellationToken = default)
    {
        await _user.InsertOneAsync(user, new InsertOneOptions(), cancellationToken);
        return user;
    }

    public async Task DeleteUserAsync(string id, CancellationToken cancellationToken = default)
    {
        await _user.DeleteOneAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _user.Find(u => u.Email == email).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<User?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _user.Find(u => u.Id == id).FirstOrDefaultAsync(cancellationToken);
    }


    public async Task<User> PatchUserAsync(
        string id,
        string email,
        string? firstName,
        string? lastName,
        CancellationToken cancellationToken = default
    )
    {
        var updates = new List<UpdateDefinition<User>>
        {
            Builders<User>.Update.Set(u => u.Email, email)
        };

        if (firstName is not null)
        {
            updates.Add(Builders<User>.Update.Set(u => u.FirstName, firstName));
        }

        if (lastName is not null)
        {
            updates.Add(Builders<User>.Update.Set(u => u.LastName, lastName));
        }

        return await _user.FindOneAndUpdateAsync(
            u => u.Id == id,
            Builders<User>.Update.Combine(updates),
            new FindOneAndUpdateOptions<User> { ReturnDocument = ReturnDocument.After },
            cancellationToken
        );
    }

    public async Task<User> UpdateUserAsync(User user, CancellationToken cancellationToken = default)
    {
        return await _user.FindOneAndReplaceAsync(u => u.Id == user.Id, user, new FindOneAndReplaceOptions<User> { ReturnDocument = ReturnDocument.After }, cancellationToken);
    }
}
