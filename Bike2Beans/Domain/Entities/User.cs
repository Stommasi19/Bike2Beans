
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Bike2Beans.Domain.Entities;


public class User
{
    public User(Guid id, Guid authId, string email, string firstName, string lastName)
    {
        this.Id = id;
        this.AuthId = authId;
        this.Email = email;
        this.FirstName = firstName;
        this.LastName = lastName;

    }
    [BsonId(IdGenerator = typeof(GuidGenerator))]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; }

    [BsonElement("authId")]
    public Guid AuthId { get; set; }

    [BsonElement("email")]
    public string Email { get; set; } = null!;
    [BsonElement("firstName")]
    public string FirstName { get; set; } = null!;
    [BsonElement("lastName")]
    public string LastName { get; set; } = null!;


}