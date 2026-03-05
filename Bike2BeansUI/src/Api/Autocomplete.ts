import { api } from "./client";

let timeout: number | undefined;

export const getAutocomplete = (text: string): Promise<any> => {
    return new Promise((resolve) => {
        clearTimeout(timeout);

        timeout = window.setTimeout(async () => {
            const response = await api.get(
                "Api/places/Autocomplete",
                { params: { text } }
            );

            resolve(response.data);
        }, 2000);
    });
};