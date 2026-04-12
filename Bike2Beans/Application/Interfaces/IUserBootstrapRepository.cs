

using Bike2Beans.Domain.Entities;

namespace Bike2Beans.Application.Interfaces;

public interface IUserBootstrapRepository
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> GetByAuthIdAsync(Guid authId, CancellationToken cancellationToken = default);
}
