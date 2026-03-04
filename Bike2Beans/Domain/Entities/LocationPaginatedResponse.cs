
using Bike2Beans.Domain.DTOs;

namespace Bike2Beans.Domain.Entities;

public class LocationPaginatedResponse
{
    public List<CoffeeshopDto> Locations { get; set; }
    public string? NextPageToken { get; set; }
}