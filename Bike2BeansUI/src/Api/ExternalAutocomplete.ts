import { api } from "./Client";

import type { AutocompleteSuggestion } from "./Autocomplete";

export const getExternalAutocomplete = async (text: string): Promise<AutocompleteSuggestion[]> => {
    const response = await api.get<AutocompleteSuggestion[]>(
        "Api/places/ExternalLocationAutocomplete",
        { params: { text } }
    );

    return Array.isArray(response.data) ? response.data : [];
};
