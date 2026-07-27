import { useEffect, useRef } from "react";
import MapboxMap, {
    Layer,
    type MapRef,
    Marker,
    Source,
} from "react-map-gl/mapbox";
import type { CoffeeshopDto } from "../../Data/CoffeeshopDto";
import type { RouteOptionDto } from "../../Data/RouteOptionDto";
import { GetDistance } from "../Map/GetDistance";
import { MapMover } from "./MapMover";
import { toRouteFeature } from "./routeGeoJson";

type Props = {
    startLocation?: { lat: number; lng: number } | null;
    mapCenter?: { lat: number; lng: number } | null;
    onViewportSettled?: (viewport: {
        lat: number;
        lng: number;
        zoom: number;
    }) => void;
    shops: CoffeeshopDto[];
    activeId: string | null;
    setActiveId: (id: string | null) => void;
    routeOptions?: RouteOptionDto[];
    selectedRouteId?: string | null;
};

type MoveEndEvent = {
    viewState: {
        latitude: number;
        longitude: number;
        zoom: number;
    };
};

type Viewport = {
    lat: number;
    lng: number;
    zoom: number;
};

const DEFAULT_MAP_CENTER = {
    lat: 47.674,
    lng: -122.1215,
};
const VIEWPORT_SETTLE_DISTANCE_KM = 0.5;

export function MapView({
    onViewportSettled,
    startLocation,
    mapCenter: providedMapCenter,
    shops,
    activeId,
    setActiveId,
    routeOptions = [],
    selectedRouteId,
}: Props) {
    const ROUTE_COLOR = "#9a6f4d";
    const ROUTE_CASE_COLOR = "#f6eee6";
    const ROUTE_DIRECTION_COLOR = "#6f4b33";
    const ROUTE_SECONDARY_COLOR = "#c8a98c";

    const token = process.env.MAPBOX_ACCESS_TOKEN;
    const mapRef = useRef<MapRef | null>(null);
    const lastSettledViewportRef = useRef<Viewport | null>(null);
    const ignoreNextMoveEndRef = useRef(false);
    const mapCenter = startLocation ?? providedMapCenter ?? DEFAULT_MAP_CENTER;
    const visibleShops = Array.isArray(shops) ? shops : [];
    const visibleRouteOptions = Array.isArray(routeOptions) ? routeOptions : [];

    useEffect(() => {
        if (!startLocation) return;
        lastSettledViewportRef.current = {
            lat: startLocation.lat,
            lng: startLocation.lng,
            zoom: 11.5,
        };
    }, [startLocation]);

    useEffect(() => {
        if (!startLocation) return;
        if (!mapRef.current) return;

        const map = mapRef.current.getMap();

        const focusUserLocation = () => {
            ignoreNextMoveEndRef.current = true;
            map.easeTo({
                center: [startLocation.lng, startLocation.lat],
                zoom: 11.5,
                duration: 1000,
            });
        };

        if (map.loaded()) {
            focusUserLocation();
            return;
        }

        map.once("load", focusUserLocation);
    }, [startLocation]);

    useEffect(() => {
        if (!activeId) return;
        if (!mapRef.current) return;

        const selected = visibleShops.find((shop) => shop.placeId === activeId);
        if (!selected) return;

        const map = mapRef.current.getMap();
        const center = map.getCenter();
        ignoreNextMoveEndRef.current = true;
        MapMover(map, center, selected);
    }, [activeId, visibleShops]);

    const rememberViewport = (viewport: Viewport) => {
        lastSettledViewportRef.current = viewport;
    };

    const handleMoveEnd = (event: MoveEndEvent) => {
        const nextViewport: Viewport = {
            lat: event.viewState.latitude,
            lng: event.viewState.longitude,
            zoom: event.viewState.zoom,
        };

        if (ignoreNextMoveEndRef.current) {
            ignoreNextMoveEndRef.current = false;
            rememberViewport(nextViewport);
            return;
        }

        const lastViewport = lastSettledViewportRef.current;
        if (!lastViewport) {
            rememberViewport(nextViewport);
            return;
        }

        const distance = GetDistance(
            lastViewport.lat,
            lastViewport.lng,
            nextViewport.lat,
            nextViewport.lng,
        );

        if (distance <= VIEWPORT_SETTLE_DISTANCE_KM) return;

        rememberViewport(nextViewport);
        onViewportSettled?.(nextViewport);
    };

    const selectedRoute =
        visibleRouteOptions.find((routeOption) => routeOption.id === selectedRouteId) ??
        visibleRouteOptions[0];
    const selectedRouteFeature = selectedRoute
        ? toRouteFeature(selectedRoute)
        : null;
    const secondaryRoutes = visibleRouteOptions.filter(
        (routeOption) => routeOption.id !== selectedRoute?.id,
    );

    return (
        <div style={{ height: "100vh", width: "100vw" }}>
            <MapboxMap
                ref={mapRef}
                mapboxAccessToken={token}
                initialViewState={{
                    latitude: mapCenter.lat,
                    longitude: mapCenter.lng,
                    zoom: 13,
                }}
                mapStyle="mapbox://styles/mapbox/streets-v12"
                onMoveEnd={handleMoveEnd}
                onError={(event) => console.error("Mapbox error", event.error)}
            >
                {startLocation ? (
                    <Marker
                        latitude={startLocation.lat}
                        longitude={startLocation.lng}
                        anchor="center"
                    >
                        <div
                            title="Approximate location"
                            style={{
                                width: 16,
                                height: 16,
                                borderRadius: "9999px",
                                background: "#2563eb",
                                border: "3px solid white",
                                boxShadow: "0 0 0 6px rgba(37, 99, 235, 0.2)",
                            }}
                        />
                    </Marker>
                ) : null}

                {visibleShops.map((shop) => (
                    <Marker
                        key={shop.placeId}
                        latitude={shop.lat}
                        longitude={shop.lng}
                        anchor="bottom"
                    >
                        <button
                            type="button"
                            onClick={(event) => {
                                event.stopPropagation();
                                setActiveId(shop.placeId);
                            }}
                            className={shop.placeId === activeId ? "pin pin-active" : "pin"}
                        />
                    </Marker>
                ))}

                {secondaryRoutes.map((routeOption) => {
                    const routeFeature = toRouteFeature(routeOption);

                    if (!routeFeature) return null;

                    return (
                        <Source
                            key={routeOption.id}
                            id={`route-source-${routeOption.id}`}
                            type="geojson"
                            data={routeFeature}
                        >
                            <Layer
                                id={`route-line-${routeOption.id}`}
                                type="line"
                                paint={{
                                    "line-color": ROUTE_SECONDARY_COLOR,
                                    "line-width": 4,
                                    "line-opacity": 0.9,
                                }}
                            />
                        </Source>
                    );
                })}

                {selectedRouteFeature ? (
                    <Source
                        id={`route-source-${selectedRoute?.id}-selected`}
                        type="geojson"
                        data={selectedRouteFeature}
                    >
                        <Layer
                            id={`route-line-case-${selectedRoute?.id}`}
                            type="line"
                            paint={{
                                "line-color": ROUTE_CASE_COLOR,
                                "line-width": 8,
                                "line-opacity": 0.95,
                            }}
                        />
                        <Layer
                            id={`route-line-${selectedRoute?.id}`}
                            type="line"
                            paint={{
                                "line-color": ROUTE_COLOR,
                                "line-width": 5,
                                "line-opacity": 0.98,
                            }}
                        />
                        <Layer
                            id={`route-direction-${selectedRoute?.id}`}
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
            </MapboxMap>
        </div>
    );
}
