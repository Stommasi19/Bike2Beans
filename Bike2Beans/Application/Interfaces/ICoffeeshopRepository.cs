

using Bike2Beans.Models.Entities;

namespace Bike2Beans.Application.Interfaces;

public interface ICoffeeshopRepository
{
    Task<List<Coffeeshop>> GetAllAsync();
    Task<Coffeeshop> InsertAsync(Coffeeshop shop, CancellationToken ct = default);
}
