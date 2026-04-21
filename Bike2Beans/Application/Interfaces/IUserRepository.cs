

using Bike2Beans.Domain.Entities;
using MongoDB.Driver;

namespace Bike2Beans.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User> CreateUserAsync(User user, CancellationToken cancellationToken = default);
        Task<User?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

        Task<User> UpdateUserAsync(User user, CancellationToken cancellationToken = default);
        Task<User> PatchUserAsync(
            string id,
            string email,
            string? firstName,
            string? lastName,
            CancellationToken cancellationToken = default
        );
        Task DeleteUserAsync(string id, CancellationToken cancellationToken = default);
    }
}
