import { useEffect, useState } from "react";
import { getExternalAutocomplete } from "../../Api/ExternalAutocomplete";
import { searchPlacesByText } from "../../Api/places";
import type { LocationChoice } from "./locationSearch";

type AutocompleteItem = {
    text?: string | null;
    Text?: string | null;
};

function normalizeSuggestions(items: AutocompleteItem[]): string[] {
    const seen = new Set<string>();
    const output: string[] = [];

    for (const item of items) {
        const raw = (item.text ?? item.Text ?? "").trim();
        if (!raw) continue;

        const key = raw.toLowerCase();
        if (seen.has(key)) continue;

        seen.add(key);
        output.push(raw);
    }

    return output;
}

function useAutocompleteSuggestions(args: {
    query: string;
    isEnabled: boolean;
    excludedSuggestion?: string;
}) {
    const { query, isEnabled, excludedSuggestion } = args;
    const [suggestions, setSuggestions] = useState<string[]>([]);

    useEffect(() => {
        const normalizedQuery = query.trim();
        if (!isEnabled || normalizedQuery.length < 2) {
            setSuggestions([]);
            return;
        }

        let canceled = false;

        const timer = setTimeout(async () => {
            try {
                const response = (await getExternalAutocomplete(normalizedQuery)) as AutocompleteItem[];
                if (canceled) return;

                const normalizedSuggestions = normalizeSuggestions(response);
                const filteredSuggestions = excludedSuggestion
                    ? normalizedSuggestions.filter(
                        (suggestion) => suggestion.toLowerCase() !== excludedSuggestion.toLowerCase()
                    )
                    : normalizedSuggestions;

                setSuggestions(filteredSuggestions);
            } catch {
                if (!canceled) {
                    setSuggestions([]);
                }
            }
        }, 250);

        return () => {
            canceled = true;
            clearTimeout(timer);
        };
    }, [excludedSuggestion, isEnabled, query]);

    return [suggestions, setSuggestions] as const;
}

async function resolveSuggestionToLocation(suggestion: string): Promise<LocationChoice | null> {
    const places = await searchPlacesByText(suggestion, false);

    if (!places?.locations?.length) {
        return null;
    }

    return places.locations[0] as LocationChoice;
}

export function useRouteSetupLocations() {
    const [startLocation, setStartLocation] = useState<LocationChoice | null>(null);
    const [stopLocation, setStopLocation] = useState<LocationChoice | null>(null);

    const [startQuery, setStartQuery] = useState("");
    const [stopQuery, setStopQuery] = useState("");

    const [isEditingStart, setIsEditingStart] = useState(true);
    const [isEditingStop, setIsEditingStop] = useState(false);

    const [startSuggestions, setStartSuggestions] = useAutocompleteSuggestions({
        query: startQuery,
        isEnabled: isEditingStart,
    });
    const [stopSuggestions, setStopSuggestions] = useAutocompleteSuggestions({
        query: stopQuery,
        isEnabled: isEditingStop,
        excludedSuggestion: startLocation?.name,
    });

    function resetStartSearch(nextQuery = "") {
        setStartQuery(nextQuery);
        setStartSuggestions([]);
    }

    function resetStopSearch(nextQuery = "") {
        setStopQuery(nextQuery);
        setStopSuggestions([]);
    }

    function clearStopLocation() {
        setStopLocation(null);
        resetStopSearch();
        setIsEditingStop(false);
    }

    async function selectStartSuggestion(suggestion: string) {
        resetStartSearch(suggestion);

        const location = await resolveSuggestionToLocation(suggestion);
        if (!location) return;

        setStartLocation(location);
        resetStartSearch();
        setIsEditingStart(false);

        if (stopLocation?.id === location.id) {
            clearStopLocation();
        }
    }

    async function selectStopSuggestion(suggestion: string) {
        resetStopSearch(suggestion);

        const location = await resolveSuggestionToLocation(suggestion);
        if (!location) return;

        setStopLocation(location);
        resetStopSearch();
        setIsEditingStop(false);
    }

    function beginStartEdit() {
        resetStartSearch(startLocation?.name ?? "");
        setIsEditingStart(true);
    }

    function beginStopEdit() {
        resetStopSearch(stopLocation?.name ?? "");
        setIsEditingStop(true);
    }

    function cancelStartEdit() {
        resetStartSearch();
        setIsEditingStart(false);
    }

    function addStopLocation() {
        resetStopSearch();
        setIsEditingStop(true);
    }

    function removeStopLocation() {
        clearStopLocation();
    }

    function cancelStopEdit() {
        resetStopSearch();
        setIsEditingStop(false);
    }

    return {
        startLocation,
        stopLocation,
        startQuery,
        stopQuery,
        isEditingStart,
        isEditingStop,
        startSuggestions,
        stopSuggestions,
        setStartQuery,
        setStopQuery,
        selectStartSuggestion,
        selectStopSuggestion,
        beginStartEdit,
        beginStopEdit,
        cancelStartEdit,
        addStopLocation,
        removeStopLocation,
        cancelStopEdit,
    };
}
