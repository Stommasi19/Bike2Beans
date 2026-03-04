

using Bike2Beans.Domain.Interfaces;

namespace Bike2Beans.Domain.DTOs;


public class PaginationSupportedCoffeeshopResultDto
{
    public List<CoffeeshopDto> Locations { get; set; }
    public string? NextPageToken { get; set; }

}

