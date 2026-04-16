import { useEffect, useState } from "react";
import { getExternalAutocomplete } from "../../Api/ExternalAutocomplete";
import { searchPlacesByText } from "../../Api/Places";
import type { RouteDto } from "../../Data/RouteDto";
import type { LocationChoice } from "./locationSearch";

type Args = {
    routeStops: RouteDto[];
};

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

export function useRouteSetupLocations({ routeStops: _routeStops }: Args) {
    const [startLocation, setStartLocation] = useState<LocationChoice | null>(null);
    const [stopLocation, setStopLocation] = useState<LocationChoice | null>(null);

    const [startQuery, setStartQuery] = useState("");
    const [stopQuery, setStopQuery] = useState("");

    const [isEditingStart, setIsEditingStart] = useState(true);
    const [isEditingStop, setIsEditingStop] = useState(false);

    const [startSuggestions, setStartSuggestions] = useState<string[]>([]);
    const [stopSuggestions, setStopSuggestions] = useState<string[]>([]);

    async function resolveSuggestionToLocation(suggestion: string): Promise<LocationChoice | null> {
        const places = await searchPlacesByText(suggestion, false);
        if (!places || places.length === 0) return null;
        return places.locations[0] as LocationChoice;
    }

    async function selectStartSuggestion(suggestion: string) {
        setStartQuery(suggestion);
        setStartSuggestions([]);

        const location = await resolveSuggestionToLocation(suggestion);
        if (!location) return;

        setStartLocation(location);
        setStartQuery("");
        setIsEditingStart(false);

        if (stopLocation?.id === location.id) {
            setStopLocation(null);
            setStopQuery("");
            setStopSuggestions([]);
            setIsEditingStop(false);
        }
    }

    async function selectStopSuggestion(suggestion: string) {
        setStopQuery(suggestion);
        setStopSuggestions([]);

        const location = await resolveSuggestionToLocation(suggestion);
        if (!location) return;

        setStopLocation(location);
        setStopQuery("");
        setIsEditingStop(false);
    }

    function beginStartEdit() {
        setStartQuery(startLocation?.name ?? "");
        setIsEditingStart(true);
    }

    function beginStopEdit() {
        setStopQuery(stopLocation?.name ?? "");
        setIsEditingStop(true);
    }

    function cancelStartEdit() {
        setStartQuery("");
        setStartSuggestions([]);
        setIsEditingStart(false);
    }

    function addStopLocation() {
        setStopQuery("");
        setStopSuggestions([]);
        setIsEditingStop(true);
    }

    function removeStopLocation() {
        setStopLocation(null);
        setStopQuery("");
        setStopSuggestions([]);
        setIsEditingStop(false);
    }

    function cancelStopEdit() {
        setStopQuery("");
        setStopSuggestions([]);
        setIsEditingStop(false);
    }

    useEffect(() => {
        const query = startQuery.trim();
        if (!isEditingStart || query.length < 2) {
            setStartSuggestions([]);
            return;
        }

        let canceled = false;

        const timer = setTimeout(async () => {
            try {
                const response = (await getExternalAutocomplete(query)) as AutocompleteItem[];
                if (canceled) return;
                setStartSuggestions(normalizeSuggestions(response));
            } catch {
                if (!canceled) setStartSuggestions([]);
            }
        }, 250);

        return () => {
            canceled = true;
            clearTimeout(timer);
        };
    }, [startQuery, isEditingStart]);

    useEffect(() => {
        const query = stopQuery.trim();
        if (!isEditingStop || query.length < 2) {
            setStopSuggestions([]);
            return;
        }

        let canceled = false;

        const timer = setTimeout(async () => {
            try {
                const response = (await getExternalAutocomplete(query)) as AutocompleteItem[];
                if (canceled) return;

                const filtered = normalizeSuggestions(response).filter(
                    (suggestion) => suggestion.toLowerCase() !== (startLocation?.name ?? "").toLowerCase()
                );

                setStopSuggestions(filtered);
            } catch {
                if (!canceled) setStopSuggestions([]);
            }
        }, 250);

        return () => {
            canceled = true;
            clearTimeout(timer);
        };
    }, [stopQuery, isEditingStop, startLocation?.name]);

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
