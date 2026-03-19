import { api } from "./client";

// src/Api/places.ts
export const searchPlacesByText = async (text: string, coffeeOnly = true) => {
    if (!text.trim()) return [];
    const { data } = await api.get("Api/places/Text", {
        params: { Text: text, PageSize: 6, coffeeOnly },
    });
    return data.locations ?? []; // adjust if your casing is different
};
