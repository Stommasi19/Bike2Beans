import { api } from "./client";
import type { CoffeeshopDto } from "../Data/CoffeeshopDto";

type CoffeeshopListResponse = {
    locations?: CoffeeshopDto[];
    Locations?: CoffeeshopDto[];
};

export function toCoffeeShopList(data: unknown): CoffeeshopDto[] {
    if (Array.isArray(data)) return data;

    if (typeof data !== "object" || data === null) return [];

    const response = data as CoffeeshopListResponse;
    const locations = response?.locations ?? response?.Locations;
    return Array.isArray(locations) ? locations : [];
}

export const GetCoffeeShops = async () => {
    const response = await api.get("api/coffeeshops");
    return toCoffeeShopList(response.data);
};
