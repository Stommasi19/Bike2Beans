import type { CoffeeshopDto } from "../../Data/CoffeeshopDto";
import type { ExternalLocationDto } from "../../Data/ExternalLocationDto";
import type { RouteDto } from "../../Data/RouteDto";

export type LocationChoice = ExternalLocationDto | CoffeeshopDto;

const fallbackLocations: ExternalLocationDto[] = [
    {
        id: "default-loc-gas-works",
        name: "Gas Works Park",
        address: "2101 N Northlake Way, Seattle, WA",
        lat: 47.6456,
        lng: -122.3344,
    },
    {
        id: "default-loc-pike",
        name: "Pike Place Market",
        address: "85 Pike St, Seattle, WA",
        lat: 47.6097,
        lng: -122.3425,
    },
    {
        id: "default-loc-green-lake",
        name: "Green Lake Park",
        address: "7201 East Green Lake Dr N, Seattle, WA",
        lat: 47.6793,
        lng: -122.3274,
    },
];

export function buildLocationCatalog(routeStops: RouteDto[]): LocationChoice[] {
    const byId = new Map<string, LocationChoice>();

    for (const stop of routeStops) {
        byId.set(stop.shop.id, stop.shop);
    }

    for (const location of fallbackLocations) {
        if (!byId.has(location.id)) {
            byId.set(location.id, location);
        }
    }

    return Array.from(byId.values());
}

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
