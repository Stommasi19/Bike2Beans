

namespace Bike2Beans.Application.Interfaces;

public interface IUser
{
    string Id { get; set; }
    string Email { get; set; }
    string FirstName { get; set; }

    string LastName { get; set; }
}
