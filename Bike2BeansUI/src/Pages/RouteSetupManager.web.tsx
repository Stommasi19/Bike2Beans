import { useState } from "react";
import type { RouteDto } from "../Data/RouteDto";
import type { CoffeeshopDto } from "../Data/CoffeeshopDto";
import { LocationBox } from "../Features/Route/LocationBox";
import { LocationSearchCard } from "../Features/RouteSetup/LocationSearchCard";
import type { LocationChoice } from "../Features/RouteSetup/locationSearch";
import { SelectedLocationCard } from "../Features/RouteSetup/SelectedLocationCard";
import { useRouteSetupLocations } from "../Features/RouteSetup/useRouteSetupLocations";
import { CreateRouteAndReturnPath } from "../Api/CreateRoute";
import { type RouteGeoJson, toRouteFeature } from "../Features/Map/routeGeoJson";
import { convertGeoJSONToGPX } from "../Features/Route/gpx";
import type { RouteOptionDto } from "../Data/RouteOptionDto";
import { RouteStopLocationType, type RouteStopDto as RouteStopPayloadDto } from "../Data/RouteStopDto";

type Props = {
    routeStops: RouteDto[];
    setRouteStops: (next: RouteDto[]) => void;
    routeOptions: RouteOptionDto[];
    setRouteOptions: (routeOptions: RouteOptionDto[]) => void;
    selectedRouteId: string | null;
    setSelectedRouteId: (routeId: string | null) => void;
};

type StartLocationSectionMode = "hidden" | "search" | "selected";
type StopLocationSectionMode = "hidden" | "add" | "search" | "selected";

function getSelectedRoute(routeOptions: RouteOptionDto[], selectedRouteId: string | null) {
    return routeOptions.find((routeOption) => routeOption.id === selectedRouteId) ?? routeOptions[0] ?? null;
}

function downloadRouteAsGpx(route: RouteGeoJson) {
    const gpx = convertGeoJSONToGPX(route);
    const blob = new Blob([gpx], { type: "application/gpx+xml" });
    const url = URL.createObjectURL(blob);

    const link = document.createElement("a");
    link.href = url;
    link.download = "this-route.gpx";
    link.click();

    URL.revokeObjectURL(url);
}

function formatDistance(distanceMeters: number) {
    const miles = distanceMeters / 1609.344;
    return `${miles.toFixed(1)} mi`;
}

function formatDuration(durationSeconds: number) {
    const totalMinutes = Math.round(durationSeconds / 60);
    const hours = Math.floor(totalMinutes / 60);
    const minutes = totalMinutes % 60;

    if (hours === 0) {
        return `${totalMinutes} min`;
    }

    if (minutes === 0) {
        return `${hours} hr`;
    }

    return `${hours} hr ${minutes} min`;
}

function getRouteLabel(index: number) {
    return index === 0 ? "Main route" : `Alternate ${index}`;
}

function isCoffeeshop(location: RouteDto["shop"]): location is CoffeeshopDto {
    return "placeId" in location;
}

function toRouteStopPayload(routeStop: RouteDto): RouteStopPayloadDto {
    const { stopId, shop } = routeStop;

    return {
        id: stopId,
        placeId: isCoffeeshop(shop) ? shop.placeId : `custom-stop-${stopId}`,
        name: shop.name,
        address: shop.address ?? "",
        locationType: isCoffeeshop(shop)
            ? RouteStopLocationType.Coffeeshop
            : RouteStopLocationType.Other,
        lat: Number(shop.lat),
        lng: Number(shop.lng),
    };
}

function buildRoutePayload(args: {
    startLocation: LocationChoice;
    stopLocation: LocationChoice | null;
    routeStops: RouteDto[];
}): Parameters<typeof CreateRouteAndReturnPath>[0] {
    const { startLocation, stopLocation, routeStops } = args;

    return {
        StartLocation: [startLocation.lat, startLocation.lng],
        EndLocation: [
            stopLocation?.lat ?? startLocation.lat,
            stopLocation?.lng ?? startLocation.lng,
        ],
        RouteStops: routeStops.map(toRouteStopPayload),
    };
}

function getDrawableRouteOptions(routeOptions: RouteOptionDto[]) {
    return routeOptions.filter((routeOption) => toRouteFeature(routeOption) !== null);
}

export function RouteSetupManager({
    routeStops,
    setRouteStops,
    routeOptions,
    setRouteOptions,
    selectedRouteId,
    setSelectedRouteId,
}: Props) {
    const [isGeneratingRoute, setIsGeneratingRoute] = useState(false);
    const [routeError, setRouteError] = useState<string | null>(null);
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
    } = useRouteSetupLocations();

    const selectedRoute = getSelectedRoute(routeOptions, selectedRouteId);
    const selectedRoutePath = selectedRoute ? toRouteFeature(selectedRoute) : null;
    const hasAlternateRoutes = routeOptions.length > 1;
    const startLocationSectionMode: StartLocationSectionMode = !routeStops.length
        ? "hidden"
        : (isEditingStart || !startLocation)
            ? "search"
            : "selected";
    const stopLocationSectionMode: StopLocationSectionMode = !startLocation
        ? "hidden"
        : isEditingStop
            ? "search"
            : stopLocation
                ? "selected"
                : "add";

    function removeStop(stopId: string) {
        setRouteStops(routeStops.filter((stop) => stop.stopId !== stopId));
    }

    function reorderStops(next: RouteDto[]) {
        setRouteStops(next);
    }

    function clearRouteSelection() {
        setRouteOptions([]);
        setSelectedRouteId(null);
    }

    function selectPrimaryRoute(nextRouteOptions: RouteOptionDto[]) {
        setRouteOptions(nextRouteOptions);
        setSelectedRouteId(nextRouteOptions[0]?.id ?? null);
    }

    function handleDownloadGPX() {
        if (!selectedRoutePath) return;
        downloadRouteAsGpx(selectedRoutePath);
    }

    function renderStartLocationSection() {
        switch (startLocationSectionMode) {
            case "hidden":
                return null;
            case "selected":
                if (!startLocation) return null;

                return (
                    <SelectedLocationCard
                        label="Start location"
                        location={startLocation}
                        onChange={beginStartEdit}
                    />
                );
            case "search":
                return (
                    <LocationSearchCard
                        label="Start location"
                        query={startQuery}
                        suggestions={startSuggestions}
                        onQueryChange={setStartQuery}
                        onSelectSuggestion={selectStartSuggestion}
                        onCancel={startLocation ? cancelStartEdit : undefined}
                    />
                );
        }
    }

    function renderStopLocationSection() {
        switch (stopLocationSectionMode) {
            case "hidden":
                return null;
            case "add":
                return (
                    <button type="button" className="btn-secondary route-stop-toggle" onClick={addStopLocation}>
                        Add stop location
                    </button>
                );
            case "search":
                return (
                    <LocationSearchCard
                        label="Stop location"
                        query={stopQuery}
                        suggestions={stopSuggestions}
                        onQueryChange={setStopQuery}
                        onSelectSuggestion={selectStopSuggestion}
                        onCancel={cancelStopEdit}
                    />
                );
            case "selected":
                if (!stopLocation) return null;

                return (
                    <SelectedLocationCard
                        label="Stop location"
                        location={stopLocation}
                        onChange={beginStopEdit}
                        onRemove={removeStopLocation}
                    />
                );
        }
    }

    async function handleSeeRoute() {
        if (!startLocation || isGeneratingRoute) return;

        try {
            setIsGeneratingRoute(true);
            setRouteError(null);
            setRouteOptions([]);

            const nextRouteOptions = await CreateRouteAndReturnPath(
                buildRoutePayload({ startLocation, stopLocation, routeStops })
            );
            const drawableRouteOptions = getDrawableRouteOptions(nextRouteOptions);

            if (drawableRouteOptions.length === 0) {
                clearRouteSelection();
                setRouteError("No drawable route was returned. Try a different start or stop location.");
                return;
            }

            selectPrimaryRoute(drawableRouteOptions);
        } catch (error) {
            setRouteError("Route generation failed. Please try again.");
        } finally {
            setIsGeneratingRoute(false);
        }
    }

    return (
        <div className="route-table-container">
            <div className="route-manager-shell">
                <div className="route-container route-setup-builder">
                    {renderStartLocationSection()}
                    <LocationBox routeStops={routeStops} reorderStops={reorderStops} removeStop={removeStop} />
                    {renderStopLocationSection()}
                </div>
                {startLocation ? (
                    <div className="route-actions">
                        <button className="btn btn-primary route-action-primary" onClick={handleSeeRoute} disabled={isGeneratingRoute}>
                            {isGeneratingRoute ? "Finding route..." : "Show Route"}
                        </button>
                        {selectedRoutePath ? (
                            <button className="btn btn-secondary route-action-secondary" onClick={handleDownloadGPX} disabled={isGeneratingRoute}>
                                Download GPX
                            </button>
                        ) : null}
                    </div>
                ) : null}
                {routeError ? <p className="form-error route-error" role="alert">{routeError}</p> : null}
                {hasAlternateRoutes ? (
                    <section className="route-options-panel">
                        <div className="route-options-header">
                            <div>
                                <p className="route-options-label">Route options</p>
                                <h3 className="route-options-title">Switch the main line</h3>
                            </div>
                            <span className="route-options-count">{routeOptions.length} shown</span>
                        </div>
                        <div className="route-options-list">
                            {routeOptions.map((routeOption, index) => {
                                const isSelected = routeOption.id === (selectedRoute?.id ?? null);

                                return (
                                    <button
                                        key={routeOption.id}
                                        type="button"
                                        className="route-option-button"
                                        data-active={isSelected}
                                        onClick={() => setSelectedRouteId(routeOption.id)}
                                        aria-pressed={isSelected}
                                    >
                                        <span className="route-option-swatch" data-active={isSelected} />
                                        <span className="route-option-copy">
                                            <span className="route-option-eyebrow">{getRouteLabel(index)}</span>
                                            <span className="route-option-summary">
                                                {formatDistance(routeOption.distanceMeters)} • {formatDuration(routeOption.durationSeconds)}
                                            </span>
                                        </span>
                                    </button>
                                );
                            })}
                        </div>
                    </section>
                ) : null}
            </div>
        </div>
    );
}
