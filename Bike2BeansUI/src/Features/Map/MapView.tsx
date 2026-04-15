import Map, { Layer, Marker, Source } from "react-map-gl/mapbox";
import { CoffeeshopDto } from "../../Data/CoffeeshopDto";
import { useEffect, useRef } from "react";
import { GetDistance } from '../Map/GetDistance'
import { RouteGeoJson } from "./routeGeoJson";
import { useFocusEffect } from "@react-navigation/native";
import { MapMover } from "./MapMover";



type Props = {
    startLocation?: { lat: number, lng: number } | null,
    onViewportSettled?: (viewport: { lat: number, lng: number, zoom: number }) => void;
    shops: CoffeeshopDto[],
    activeId: string | null,
    setActiveId: (id: string) => void;
    routePath?: RouteGeoJson | null

}
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

const VIEWPORT_SETTLE_DISTANCE_KM = 0.5;



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
            suppressedViewportRef.current = {
                lat: startLocation.lat,
                lng: startLocation.lng,
                zoom: 11.5,
            };

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

        const selected = shops.find((s) => s.placeId === activeId);
        if (!selected) return;

        const map = mapRef.current.getMap();
        const center = map.getCenter();
        MapMover(map, center, selected)
    }, [activeId, shops]);


    const rememberViewport = (viewport: Viewport) => {
        lastSettledViewportRef.current = viewport;
    };
    const ignoreNextMoveEndRef = useRef(false);

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
            nextViewport.lng
        );

        if (distance <= VIEWPORT_SETTLE_DISTANCE_KM) return;

        rememberViewport(nextViewport);
        onViewportSettled?.(nextViewport);
        if (shops.filter((shop) => shop.placeId !== activeId)) {
            setActiveId("null")
        }
    };

    if (!startLocation) {
        return <div style={{ height: "100vh", width: "100vw" }} />;
    }
    return (
        <div style={{ height: "100vh", width: "100vw" }}>
            <Map
                ref={mapRef}
                mapboxAccessToken={token}
                initialViewState={{
                    latitude: startLocation?.lat,
                    longitude: startLocation?.lng,
                    zoom: 13,
                }}
                mapStyle="mapbox://styles/mapbox/streets-v12"
                onMoveEnd={handleMoveEnd}
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
