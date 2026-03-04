import { useMemo, useState } from "react";
import type { RouteDto } from "../../Data/RouteDto";
import {
    buildLocationCatalog,
    searchLocations,
    type LocationChoice,
} from "./locationSearch";

type Args = {
    routeStops: RouteDto[];
};

export function useRouteSetupLocations({ routeStops }: Args) {
    const [startLocation, setStartLocation] = useState<LocationChoice | null>(null);
    const [stopLocation, setStopLocation] = useState<LocationChoice | null>(null);

    const [startQuery, setStartQuery] = useState("");
    const [stopQuery, setStopQuery] = useState("");

    const [isEditingStart, setIsEditingStart] = useState(true);
    const [isEditingStop, setIsEditingStop] = useState(false);

    const locationCatalog = useMemo(() => buildLocationCatalog(routeStops), [routeStops]);
    const startResults = useMemo(() => searchLocations(locationCatalog, startQuery), [locationCatalog, startQuery]);
    const stopResults = useMemo(() => {
        const stopCandidates = locationCatalog.filter((location) => location.id !== startLocation?.id);
        return searchLocations(stopCandidates, stopQuery);
    }, [locationCatalog, startLocation?.id, stopQuery]);

    function selectStartLocation(location: LocationChoice) {
        setStartLocation(location);
        setStartQuery("");
        setIsEditingStart(false);

        if (stopLocation?.id === location.id) {
            setStopLocation(null);
            setStopQuery("");
            setIsEditingStop(false);
        }
    }

    function selectStopLocation(location: LocationChoice) {
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
        setIsEditingStart(false);
    }

    function addStopLocation() {
        setStopQuery("");
        setIsEditingStop(true);
    }

    function removeStopLocation() {
        setStopLocation(null);
        setStopQuery("");
        setIsEditingStop(false);
    }

    function cancelStopEdit() {
        setStopQuery("");
        setIsEditingStop(false);
    }

    return {
        startLocation,
        stopLocation,
        startQuery,
        stopQuery,
        isEditingStart,
        isEditingStop,
        startResults,
        stopResults,
        setStartQuery,
        setStopQuery,
        selectStartLocation,
        selectStopLocation,
        beginStartEdit,
        beginStopEdit,
        cancelStartEdit,
        addStopLocation,
        removeStopLocation,
        cancelStopEdit,
    };
}
