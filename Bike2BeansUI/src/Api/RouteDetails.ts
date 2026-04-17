import { api } from "./Client";

type RawSavedRoutePreview = {
    id?: string;
    Id?: string;
    name?: string;
    Name?: string;
};

export type SavedRoutePreview = {
    id: string;
    name: string;
};

export function normalizeSavedRoutePreview(route: RawSavedRoutePreview): SavedRoutePreview | null {
    const id = route.id ?? route.Id;
    if (!id) return null;

    return {
        id: String(id),
        name: route.name ?? route.Name ?? "Untitled route",
    };
}

export const GetRoutes = async (): Promise<SavedRoutePreview[]> => {
    const response = await api.get<RawSavedRoutePreview[]>("api/Route");
    const routes = Array.isArray(response.data) ? response.data : [];

    return routes
        .map(normalizeSavedRoutePreview)
        .filter((route): route is SavedRoutePreview => route !== null);
};
