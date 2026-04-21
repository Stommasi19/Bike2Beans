import { CoffeeshopDto } from "../Data/CoffeeshopDto";
import { ExternalLocationDto } from "../Data/ExternalLocationDto";
import { normalizeRouteOption, RouteOptionDto } from "../Data/RouteOptionDto";
import type { RouteStopDto } from "../Data/RouteStopDto";
import { api } from "./client";

export type Props = {
    StartLocation: [number, number],
    EndLocation?: [number, number],
    RouteStops: RouteStopDto[]
}

export const CreateRouteAndReturnPath = async (payload: Props): Promise<RouteOptionDto[]> => {
    const response = await api.post("/api/mapbox/GenerateRoute", payload)
    const routeOptions = Array.isArray(response.data) ? response.data : [];

    return routeOptions.map(normalizeRouteOption);
}
