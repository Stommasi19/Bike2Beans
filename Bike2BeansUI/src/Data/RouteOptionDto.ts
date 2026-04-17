
type RawRouteOptionDto = {
    id?: string;
    Id?: string;
    optionIndex?: number;
    OptionIndex?: number;
    distanceMeters?: number;
    DistanceMeters?: number;
    durationSeconds?: number;
    DurationSeconds?: number;
    geometryType?: string;
    GeometryType?: string;
    coordinates?: number[][];
    Coordinates?: number[][];
};

export type RouteOptionDto = {
    id: string;
    optionIndex: number;
    distanceMeters: number;
    durationSeconds: number;
    geometryType: string;
    coordinates: number[][];
};

function toNumber(value: unknown, fallback = 0): number {
    const parsed = Number(value);
    return Number.isFinite(parsed) ? parsed : fallback;
}

export function normalizeRouteOption(routeOption: RawRouteOptionDto): RouteOptionDto {
    return {
        id: routeOption.id ?? routeOption.Id ?? crypto.randomUUID(),
        optionIndex: toNumber(routeOption.optionIndex ?? routeOption.OptionIndex),
        distanceMeters: toNumber(routeOption.distanceMeters ?? routeOption.DistanceMeters),
        durationSeconds: toNumber(routeOption.durationSeconds ?? routeOption.DurationSeconds),
        geometryType: routeOption.geometryType ?? routeOption.GeometryType ?? "LineString",
        coordinates: routeOption.coordinates ?? routeOption.Coordinates ?? [],
    };
}
