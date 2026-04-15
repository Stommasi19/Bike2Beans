using MongoDB.Bson.Serialization.Attributes;

namespace Bike2Beans.Domain.Entities;


public class User
{
    public User(string id, string email, string firstName, string lastName)
    {
        this.Id = id;
        this.Email = email;
        this.FirstName = firstName;
        this.LastName = lastName;

    }
    [BsonId]
    public string Id { get; set; } = null!;

    [BsonElement("email")]
    public string Email { get; set; } = null!;
    [BsonElement("firstName")]
    public string FirstName { get; set; } = null!;
    [BsonElement("lastName")]
    public string LastName { get; set; } = null!;


}
