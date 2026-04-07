

using Bike2Beans.Application.Interfaces;

namespace Bike2Beans.Application.DTOs;


public class PaginationSupportedCoffeeshopResultDto
{
    public List<CoffeeshopDto> Locations { get; set; } = new List<CoffeeshopDto>();
    public string? NextPageToken { get; set; }

}

