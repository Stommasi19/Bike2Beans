import type { RouteOptionDto } from "../../Data/RouteOptionDto";

export type RouteGeoJson = {
    type: "Feature";
    properties: Record<string, never>;
    geometry: {
        type: "LineString";
        coordinates: number[][];
    };
};

function getRouteCoordinates(routeOption: RouteOptionDto): number[][] {
    if (!routeOption) return [];

    return (

        routeOption.Coordinates ?? []

    );
}

function normalizeRouteCoordinatePair(pair: number[]): number[] | null {
    if (!Array.isArray(pair) || pair.length < 2) return null;

    const first = Number(pair[0]);
    const second = Number(pair[1]);
    if (!Number.isFinite(first) || !Number.isFinite(second)) return null;

    const firstLooksLikeLat = Math.abs(first) <= 90;
    const secondLooksLikeLat = Math.abs(second) <= 90;

    // Mapbox expects [lng, lat]. If data arrives as [lat, lng], swap it.
    if (firstLooksLikeLat && !secondLooksLikeLat) {
        return [second, first];
    }

    return [first, second];
}

export function toRouteFeature(routeOption: RouteOptionDto): RouteGeoJson | null {
    const coordinates = getRouteCoordinates(routeOption)
        .map(normalizeRouteCoordinatePair)
        .filter((coord): coord is number[] => coord !== null);

    if (coordinates.length < 2) return null;

    return {
        type: "Feature",
        properties: {},
        geometry: {
            type: "LineString",
            coordinates,
        },
    };
}
