import { RouteProp, useRoute } from "@react-navigation/native";
import { useState } from "react";
import type { RouteDto } from "../Data/RouteDto";
import { RouteMapView } from "../Features/Map/RouteMapView";
import { LocationBox } from "../Features/Route/LocationBox";
import { LocationSearchCard } from "../Features/RouteSetup/LocationSearchCard";
import { SelectedLocationCard } from "../Features/RouteSetup/SelectedLocationCard";
import { useRouteSetupLocations } from "../Features/RouteSetup/useRouteSetupLocations";
import type { RootStackParamList } from "../Navigation/types";
import { CreateRouteAndReturnPath } from "../Api/CreateRoute";
import { type RouteGeoJson, toRouteFeature } from "../Features/Map/routeGeoJson";

type Props = {
    routeStops: RouteDto[]
    setRouteStops: (next: RouteDto[]) => void;
    routePath: RouteGeoJson | null;
    setRoutePath: (routePath: RouteGeoJson) => void;
}
export function RouteSetupManager({ routeStops, setRouteStops, routePath, setRoutePath }: Props) {

    const route = useRoute<RouteProp<RootStackParamList, "RouteSetup">>();

    const {
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
    } = useRouteSetupLocations({ routeStops });

    function removeStop(stopId: string) {
        setRouteStops(routeStops.filter((stop) => stop.stopId !== stopId));
    }

    function reorderStops(next: RouteDto[]) {
        setRouteStops(next);
    }
    async function handleSeeRoute() {
        if (!startLocation) return; // or show toast

        const payload = {
            StartLocation: [startLocation.lat, startLocation.lng] as [number, number],
            EndLocation: [
                stopLocation?.lat ?? startLocation.lat,
                stopLocation?.lng ?? startLocation.lng,
            ] as [number, number], RouteStops: routeStops.map((s) => s.shop),
        };

        try {
            const routeOptions = await CreateRouteAndReturnPath(payload);
            const routeFeature = toRouteFeature(routeOptions?.[0]);
            if (!routeFeature) {
                console.warn("No drawable route geometry returned from API.");
                return;
            }
            setRoutePath(routeFeature);

            // TODO: set state / navigate to route screen with path
            // navigation.navigate("RouteView", { path })
        } catch (e) {
            console.error(e);
        }
    }


    return (
        <div className="route-table-container center">
            <div className="route-container route-setup-builder">
                {routeStops.length > 0 && (
                    isEditingStart || !startLocation ? (
                        <LocationSearchCard
                            label="Start location"
                            query={startQuery}
                            suggestions={startSuggestions}
                            onQueryChange={setStartQuery}
                            onSelectSuggestion={selectStartSuggestion}
                            onCancel={startLocation ? cancelStartEdit : undefined}
                        />
                    ) : (
                        <SelectedLocationCard
                            label="Start location"
                            location={startLocation}
                            onChange={beginStartEdit}
                        />
                    )
                )}

                <LocationBox routeStops={routeStops} reorderStops={reorderStops} removeStop={removeStop} />

                {startLocation && !stopLocation && !isEditingStop ? (
                    <button type="button" className="btn-secondary route-stop-toggle" onClick={addStopLocation}>
                        Add stop location
                    </button>
                ) : null}

                {startLocation && isEditingStop ? (
                    <LocationSearchCard
                        label="Stop location"
                        query={stopQuery}
                        suggestions={stopSuggestions}
                        onQueryChange={setStopQuery}
                        onSelectSuggestion={selectStopSuggestion}
                        onCancel={cancelStopEdit}
                    />
                ) : null}

                {startLocation && stopLocation && !isEditingStop ? (
                    <SelectedLocationCard
                        label="Stop location"
                        location={stopLocation}
                        onChange={beginStopEdit}
                        onRemove={removeStopLocation}
                    />
                ) : null}
            </div>
            {startLocation && (
                <button className="btn btn-primary center"
                    onClick={handleSeeRoute}
                >Show Route</button>
            )}

        </div>

    );
}
