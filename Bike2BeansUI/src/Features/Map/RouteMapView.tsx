import { useEffect, useMemo, useRef } from "react";
import type { Map as MapboxMap } from "mapbox-gl";
import MapView, { Layer, Marker, Source } from "react-map-gl/mapbox";
import type { CoffeeshopDto } from "../../Data/CoffeeshopDto";
import type { ExternalLocationDto } from "../../Data/ExternalLocationDto";
import type { RouteDto } from "../../Data/RouteDto";
import {
    focusMapToLocations,
    getInitialViewStateForLocations,
    type MapLocation,
} from "./focusMapToLocations";
import type { RouteGeoJson } from "./routeGeoJson";

type Props = {
    stops: RouteDto[];
    startLocation?: ExternalLocationDto | CoffeeshopDto | null;
    stopLocation?: ExternalLocationDto | CoffeeshopDto | null;
    activeId: string | null;
    setActiveId: (id: string) => void;
    routePath?: RouteGeoJson | null;
};

function buildFocusLocations(
    stops: RouteDto[],
    startLocation?: ExternalLocationDto | CoffeeshopDto | null,
    stopLocation?: ExternalLocationDto | CoffeeshopDto | null
): MapLocation[] {
    const dedup = new globalThis.Map<string, MapLocation>();

    for (const stop of stops) {
        const key = `${stop.shop.lat.toFixed(6)}:${stop.shop.lng.toFixed(6)}`;
        dedup.set(key, { lat: stop.shop.lat, lng: stop.shop.lng });
    }

    if (startLocation) {
        const key = `${startLocation.lat.toFixed(6)}:${startLocation.lng.toFixed(6)}`;
        dedup.set(key, { lat: startLocation.lat, lng: startLocation.lng });
    }

    if (stopLocation) {
        const key = `${stopLocation.lat.toFixed(6)}:${stopLocation.lng.toFixed(6)}`;
        dedup.set(key, { lat: stopLocation.lat, lng: stopLocation.lng });
    }

    return Array.from(dedup.values());
}

const DEFAULT_INITIAL_VIEW = {
    latitude: 47.6062,
    longitude: -122.3321,
    zoom: 10,
};

const ROUTE_COLOR = "#9a6f4d";
const ROUTE_CASE_COLOR = "#f6eee6";
const ROUTE_DIRECTION_COLOR = "#6f4b33";

export function RouteMapView({ stops, startLocation, stopLocation, activeId, setActiveId, routePath }: Props) {
    const token = process.env.MAPBOX_ACCESS_TOKEN;
    const mapRef = useRef<{ getMap: () => MapboxMap } | null>(null);
    const isStartStopOverlap =
        !!startLocation &&
        !!stopLocation &&
        Math.abs(startLocation.lat - stopLocation.lat) < 0.000001 &&
        Math.abs(startLocation.lng - stopLocation.lng) < 0.000001;

    // Initial camera is based on route shops only (no start/stop), so the first visit opens centered on the route.
    const initialRouteLocations = useMemo(() => buildFocusLocations(stops, null, null), [stops]);
    const initialViewState = useMemo(() => {
        return (
            getInitialViewStateForLocations(initialRouteLocations, {
                padding: 108,
                maxZoom: 14.5,
                singlePointZoom: 13,
            }) ?? DEFAULT_INITIAL_VIEW
        );
    }, [initialRouteLocations]);

    const endpointKey = `${startLocation?.id ?? ""}:${startLocation?.lat ?? ""}:${startLocation?.lng ?? ""}|${stopLocation?.id ?? ""}:${stopLocation?.lat ?? ""}:${stopLocation?.lng ?? ""}`;

    useEffect(() => {
        // Map refocuses only when start/stop locations change.
        if (!startLocation && !stopLocation) return;

        const map = mapRef.current?.getMap();
        if (!map) return;

        const locationsForFocus = buildFocusLocations(stops, startLocation, stopLocation);
        if (locationsForFocus.length === 0) return;

        const focus = () => {
            focusMapToLocations(map, locationsForFocus, {
                padding: 108,
                maxZoom: 14.5,
                singlePointZoom: 13,
                durationMs: 1000,
                distanceThresholdKm: 0.35,
                zoomThreshold: 0.25,
            });
        };

        if (map.loaded()) {
            focus();
            return;
        }

        map.once("load", focus);
    }, [endpointKey]);

    return (
        <div style={{ height: "100vh", width: "100vw" }}>
            <MapView
                ref={mapRef}
                mapboxAccessToken={token}
                initialViewState={initialViewState}
                mapStyle="mapbox://styles/mapbox/streets-v12"
            >
                {routePath ? (
                    <Source id="route-source" type="geojson" data={routePath}>
                        <Layer
                            id="route-line-case"
                            type="line"
                            paint={{
                                "line-color": ROUTE_CASE_COLOR,
                                "line-width": 8,
                                "line-opacity": 0.95,
                            }}
                        />
                        <Layer
                            id="route-line"
                            type="line"
                            paint={{
                                "line-color": ROUTE_COLOR,
                                "line-width": 5,
                                "line-opacity": 0.98,
                            }}
                        />
                        <Layer
                            id="route-direction"
                            type="symbol"
                            layout={{
                                "symbol-placement": "line",
                                "symbol-spacing": 68,
                                "text-field": "➤",
                                "text-size": 12,
                                "text-keep-upright": false,
                                "text-allow-overlap": true,
                                "text-ignore-placement": true,
                            }}
                            paint={{
                                "text-color": ROUTE_DIRECTION_COLOR,
                                "text-halo-color": ROUTE_CASE_COLOR,
                                "text-halo-width": 1.5,
                            }}
                        />
                    </Source>
                ) : null}
                {stops.map((stop) => (
                    <Marker
                        key={stop.shop.id}
                        latitude={stop.shop.lat}
                        longitude={stop.shop.lng}
                        anchor="bottom"
                    >
                        <button
                            type="button"
                            onClick={(event) => {
                                event.stopPropagation();
                                setActiveId(stop.shop.id);
                            }}
                            className={stop.shop.id === activeId ? "pin pin-active" : "pin"}
                        />
                    </Marker>
                ))}
                {startLocation ? (
                    <Marker
                        key={`start-${startLocation.id}`}
                        latitude={startLocation.lat}
                        longitude={startLocation.lng}
                        anchor="bottom"
                        offset={isStartStopOverlap ? [-14, -6] : [0, 0]}
                    >
                        <div className="route-point-pin route-point-start" title="Start location">
                            S
                        </div>
                    </Marker>
                ) : null}
                {stopLocation ? (
                    <Marker
                        key={`stop-${stopLocation.id}`}
                        latitude={stopLocation.lat}
                        longitude={stopLocation.lng}
                        anchor="bottom"
                        offset={isStartStopOverlap ? [14, -6] : [0, 0]}
                    >
                        <div className="route-point-pin route-point-stop" title="Stop location">
                            E
                        </div>
                    </Marker>
                ) : null}
            </MapView>
        </div>
    );
}
