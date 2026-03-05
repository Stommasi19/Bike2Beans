import { api } from "./client";


export const getAutocomplete = (text: string): Promise<any> => {
    let timeout: number | undefined;

    return new Promise((resolve) => {
        clearTimeout(timeout);

        timeout = window.setTimeout(async () => {
            const response = await api.get(
                "Api/places/Autocomplete",
                { params: { text } }
            );

            resolve(response.data);
        }, 1000);
    });
};