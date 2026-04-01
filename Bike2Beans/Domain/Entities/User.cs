using Bike2Beans.Domain.Interfaces;

namespace Bike2Beans.Domain.Entities;

public class User : IUser
{
    public string Id { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
}