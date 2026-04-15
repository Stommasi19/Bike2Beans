import { CoffeeshopDto } from "../Data/CoffeeshopDto";
import { ExternalLocationDto } from "../Data/ExternalLocationDto";
import { RouteOptionDto } from "../Data/RouteOptionDto";
import { api } from "./Client";

export type Props = {
    StartLocation: [number, number],
    EndLocation?: [number, number],
    RouteStops: CoffeeshopDto[] | ExternalLocationDto[]
}

export const CreateRouteAndReturnPath = async (payload: Props): Promise<RouteOptionDto[]> => {
    const response = await api.post("/api/mapbox/GenerateRoute", payload)
    return response.data
}
