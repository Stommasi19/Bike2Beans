import { api } from "./client";

export type AutocompleteSuggestion = {
    text?: string | null;
    Text?: string | null;
};

export const getAutocomplete = async (text: string): Promise<AutocompleteSuggestion[]> => {
    const response = await api.get<AutocompleteSuggestion[]>(
        "Api/places/Autocomplete",
        { params: { text } }
    );

    return Array.isArray(response.data) ? response.data : [];
};
