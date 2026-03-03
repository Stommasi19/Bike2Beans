using Bike2Beans.Dtos;
using MediatR;

namespace Bike2Beans.Application.CoffeeShops.Queries.Get;

public record GetAllCoffeeShopQuery() : IRequest<List<CoffeeShopDto>>;
