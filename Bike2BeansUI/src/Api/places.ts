import { api } from "./client";

export const searchPlacesByText = async (text: string, coffeeOnly = true) => {
    if (!text.trim()) return [];
    const { data } = await api.get("Api/places/Text", {
        params: { Text: text, PageSize: 6, coffeeOnly },
    });
    return data ?? [];
};
export const searchPlacesNearby = async (lat: number, lng: number) => {
    const { data } = await api.get("Api/places/Nearby", {
        params: { Lat: lat, Lng: lng },
    });
    return data ?? [];
};
