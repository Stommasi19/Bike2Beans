import type { CoffeeshopDto } from "../../Data/CoffeeshopDto";
import type { ExternalLocationDto } from "../../Data/ExternalLocationDto";
import type { RouteDto } from "../../Data/RouteDto";

export type LocationChoice = ExternalLocationDto | CoffeeshopDto;




export function searchLocations(locations: LocationChoice[], query: string): LocationChoice[] {
    const normalized = query.trim().toLowerCase();
    if (!normalized) return [];

    return locations.filter((location) => {
        return (
            location.name.toLowerCase().includes(normalized) ||
            location.address.toLowerCase().includes(normalized)
        );
    });
}
