import { RouteProp, useRoute } from "@react-navigation/native";
import { useState } from "react";
import type { RouteDto } from "../Data/RouteDto";
import { RouteMapView } from "../Features/Map/RouteMapView";
import { LocationBox } from "../Features/Route/LocationBox";
import { LocationSearchCard } from "../Features/RouteSetup/LocationSearchCard";
import { SelectedLocationCard } from "../Features/RouteSetup/SelectedLocationCard";
import { useRouteSetupLocations } from "../Features/RouteSetup/useRouteSetupLocations";
import type { RootStackParamList } from "../Navigation/types";

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
    } = useRouteSetupLocations({ routeStops });

    function removeStop(stopId: string) {
        setRouteStops((prev) => prev.filter((stop) => stop.stopId !== stopId));
    }

    function reorderStops(next: RouteDto[]) {
        setRouteStops(next);
    }

    return (
        <div className="absolute h-full w-full">
            <div className="absolute inset-0">
                <RouteMapView stops={routeStops} activeId={activeId} setActiveId={setActiveId} />
            </div>
            <div className="route-table-container">
                <div className="route-container route-setup-builder">
                    <section className="panel route-setup-summary">
                        <h2 className="section-title">Route setup</h2>
                        <p className="muted">Choose where you start, then optionally set a different stop location.</p>
                    </section>
                    {isEditingStart || !startLocation ? (
                        <LocationSearchCard
                            label="Start location"
                            query={startQuery}
                            results={startResults}
                            onQueryChange={setStartQuery}
                            onSelect={selectStartLocation}
                            onCancel={startLocation ? cancelStartEdit : undefined}
                        />
                    ) : (
                        <SelectedLocationCard
                            label="Start location"
                            location={startLocation}
                            onChange={beginStartEdit}
                        />
                    )}
                    {startLocation && !stopLocation && !isEditingStop ? (
                        <button type="button" className="btn-secondary route-stop-toggle" onClick={addStopLocation}>
                            Add stop location
                        </button>
                    ) : null}
                    {startLocation && isEditingStop ? (
                        <LocationSearchCard
                            label="Stop location"
                            query={stopQuery}
                            results={stopResults}
                            onQueryChange={setStopQuery}
                            onSelect={selectStopLocation}
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
                    <LocationBox routeStops={routeStops} reorderStops={reorderStops} removeStop={removeStop} />
                </div>
            </div>
        </div>
    );
}
