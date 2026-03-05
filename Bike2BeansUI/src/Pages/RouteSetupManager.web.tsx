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

export function RouteSetupManager() {
    const [activeId, setActiveId] = useState<string | null>(null);

    const route = useRoute<RouteProp<RootStackParamList, "RouteSetup">>();
    const [routeStops, setRouteStops] = useState<RouteDto[]>(route.params?.routeStops ?? []);

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
        setRouteStops((prev) => prev.filter((stop) => stop.stopId !== stopId));
    }

    function reorderStops(next: RouteDto[]) {
        setRouteStops(next);
    }
    const [routePath, setRoutePath] = useState<RouteGeoJson | null>(null);
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
        <div className="absolute h-full w-full">
            <div className="absolute inset-0">
                <RouteMapView
                    stops={routeStops}
                    startLocation={startLocation}
                    stopLocation={stopLocation}
                    activeId={activeId}
                    setActiveId={setActiveId}
                    routePath={routePath}
                />
            </div>
            <div className="route-table-container">
                <div className="route-container route-setup-builder">


                    {isEditingStart || !startLocation ? (
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
                <button className="btn"
                    onClick={handleSeeRoute}
                >See Route</button>
            </div>

        </div>
    );
}
