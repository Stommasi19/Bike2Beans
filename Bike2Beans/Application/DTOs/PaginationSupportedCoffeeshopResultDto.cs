

using Bike2Beans.Application.Interfaces;

namespace Bike2Beans.Application.DTOs;


public class PaginationSupportedCoffeeshopResultDto
{
    public List<CoffeeshopDto> Locations { get; set; }
    public string? NextPageToken { get; set; }

}

