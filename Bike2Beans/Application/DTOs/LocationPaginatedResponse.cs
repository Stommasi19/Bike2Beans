


using Bike2Beans.Application.DTOs;

namespace Bike2Beans.Domain.Entities;

public class LocationPaginatedResponse
{
    public List<CoffeeshopDto> Locations { get; set; } = new List<CoffeeshopDto>();
    public string? NextPageToken { get; set; }
}