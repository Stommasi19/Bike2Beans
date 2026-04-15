import Map, { Layer, Marker, Source } from "react-map-gl/mapbox";
import { CoffeeshopDto } from "../../Data/CoffeeshopDto";
import { useEffect, useRef } from "react";
import { GetDistance } from '../Map/GetDistance'
import { RouteGeoJson } from "./routeGeoJson";

const DEFAULT_INITIAL_VIEW = {
    latitude: 47.6062,
    longitude: -122.3321,
    zoom: 10,
};

type Props = {
    startLocation?: { lat: number, lng: number } | null,
    onViewportSettled?: (viewport: { lat: number, lng: number, zoom: number }) => void;
    shops: CoffeeshopDto[],
    activeId: string | null,
    setActiveId: (id: string) => void;
    routePath?: RouteGeoJson | null

}


export function MapView({ onViewportSettled, startLocation, shops, activeId, setActiveId, routePath }: Props) {

    const ROUTE_COLOR = "#9a6f4d";
    const ROUTE_CASE_COLOR = "#f6eee6";
    const ROUTE_DIRECTION_COLOR = "#6f4b33";

    const token = process.env.MAPBOX_ACCESS_TOKEN;
    const mapRef = useRef<any>(null);

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

        var selected = shops.find((s) => s.id === activeId);
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
    }, [activeId, shops])


    return (
        <div style={{ height: "100vh", width: "100vw" }}>
            <Map
                ref={mapRef}
                mapboxAccessToken={token}
                initialViewState={{
                    latitude: startLocation?.lat ?? DEFAULT_INITIAL_VIEW.latitude,
                    longitude: startLocation?.lng ?? DEFAULT_INITIAL_VIEW.longitude,
                    zoom: DEFAULT_INITIAL_VIEW.zoom,
                }}
                mapStyle="mapbox://styles/mapbox/streets-v12"
                onMoveEnd={(event) => {
                    console.log("MapView onMoveEnd", event.viewState);

                    onViewportSettled?.({
                        lat: event.viewState.latitude,
                        lng: event.viewState.longitude,
                        zoom: event.viewState.zoom,
                    });
                }}

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
                            key={s.id}
                            latitude={s.lat}
                            longitude={s.lng}
                            anchor="bottom"
                        >
                            <button
                                type="button"
                                onClick={(e) => {
                                    e.stopPropagation();
                                    setActiveId(s.id);
                                }}
                                className={s.id === activeId ? "pin pin-active" : "pin"}

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
        </div>
    );
}
