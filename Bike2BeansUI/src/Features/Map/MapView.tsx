import Map, { Layer, Marker, Source } from "react-map-gl/mapbox";
import { CoffeeshopDto } from "../../Data/CoffeeshopDto";
import { useEffect, useRef } from "react";
import { GetDistance } from '../Map/GetDistance'
import { RouteGeoJson } from "./routeGeoJson";
import { useFocusEffect } from "@react-navigation/native";



type Props = {
    startLocation?: { lat: number, lng: number } | null,
    onViewportSettled?: (viewport: { lat: number, lng: number, zoom: number }) => void;
    shops: CoffeeshopDto[],
    activeId: string | null,
    setActiveId: (id: string) => void;
    routePath?: RouteGeoJson | null

}

type Viewport = {
    lat: number;
    lng: number;
    zoom: number;
};

const VIEWPORT_SETTLE_DISTANCE_KM = 0.5;
const PROGRAMMATIC_SETTLE_TOLERANCE_KM = 0.05;
const PROGRAMMATIC_ZOOM_TOLERANCE = 0.1;


export function MapView({ onViewportSettled, startLocation, shops, activeId, setActiveId, routePath }: Props) {



    const ROUTE_COLOR = "#9a6f4d";
    const ROUTE_CASE_COLOR = "#f6eee6";
    const ROUTE_DIRECTION_COLOR = "#6f4b33";

    const token = process.env.MAPBOX_ACCESS_TOKEN;
    const mapRef = useRef<any>(null);
    const lastSettledViewportRef = useRef<Viewport | null>(null);
    const suppressedViewportRef = useRef<Viewport | null>(null);


    useEffect(() => {
        if (!startLocation) return;
        lastSettledViewportRef.current = {
            lat: startLocation.lat,
            lng: startLocation.lng,
            zoom: 11.5
        };
    }, [startLocation?.lat, startLocation?.lng])
    useEffect(() => {
        if (!startLocation) return;
        if (!mapRef.current) return;

        const map = mapRef.current.getMap();
        const focusUserLocation = () => {
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
    }, [startLocation?.lat, startLocation?.lng]);

    useEffect(() => {
        if (!activeId) return;
        if (!mapRef.current) return;

        const selected = shops.find((s) => s.placeId === activeId);
        if (!selected) return;

        const map = mapRef.current.getMap();
        const center = map.getCenter();
        const distance = GetDistance(center.lat, center.lng, selected.lat, selected.lng)
        if (distance < 50) {
            map.easeTo({
                center: [selected.lng, selected.lat],
                zoom: 14,
                duration: 800,
            })
        }
        else {
            map.flyTo({
                center: [selected.lng, selected.lat],
                zoom: 14,
                duration: 8000,
            })
        }
    }, [activeId, shops]);



    return (
        <div style={{ height: "100vh", width: "100vw" }}>
            <Map
                ref={mapRef}
                mapboxAccessToken={token}
                initialViewState={{
                    latitude: startLocation?.lat,
                    longitude: startLocation?.lng,
                    zoom: 11,
                }}
                mapStyle="mapbox://styles/mapbox/streets-v12"

                onMoveEnd={(event) => {
                    const nextViewport: Viewport = {
                        lat: event.viewState.latitude,
                        lng: event.viewState.longitude,
                        zoom: event.viewState.zoom,
                    };

                    const suppressedViewport = suppressedViewportRef.current;
                    if (suppressedViewport) {
                        const suppressedDistance = GetDistance(
                            suppressedViewport.lat,
                            suppressedViewport.lng,
                            nextViewport.lat,
                            nextViewport.lng
                        );
                        const suppressedZoomDelta = Math.abs(suppressedViewport.zoom - nextViewport.zoom);

                        if (
                            suppressedDistance <= PROGRAMMATIC_SETTLE_TOLERANCE_KM &&
                            suppressedZoomDelta <= PROGRAMMATIC_ZOOM_TOLERANCE
                        ) {
                            suppressedViewportRef.current = null;
                            lastSettledViewportRef.current = nextViewport;
                            return;
                        }

                        suppressedViewportRef.current = null;
                    }

                    const lastSettledViewport = lastSettledViewportRef.current;
                    if (!lastSettledViewport) {
                        lastSettledViewportRef.current = nextViewport;
                        return;
                    }

                    const distanceFromLastSettled = GetDistance(
                        lastSettledViewport.lat,
                        lastSettledViewport.lng,
                        nextViewport.lat,
                        nextViewport.lng
                    );

                    if (distanceFromLastSettled <= VIEWPORT_SETTLE_DISTANCE_KM) {
                        return;
                    }

                    lastSettledViewportRef.current = nextViewport;
                    onViewportSettled?.(nextViewport);
                }}
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

                {
                    shops.map((s) => (
                    <Marker
                        key={s.placeId}
                        latitude={s.lat}
                        longitude={s.lng}
                        anchor="bottom"
                    >
                        <button
                            type="button"
                            onClick={(e) => {
                                e.stopPropagation();
                                setActiveId(s.placeId);
                            }}
                            className={s.placeId === activeId ? "pin pin-active" : "pin"}

                        />
                    </Marker>

                ))
                }
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
            </Map>
        </div >
    );
}
