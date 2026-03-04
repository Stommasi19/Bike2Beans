import { api } from "./client";

export const getExternalAutocomplete = async (text: string) => {
    const response = await api.get("Api/places/ExternalLocationAutocomplete", {
        params: { text },
    });
    return response.data;
};
