import Map, { Marker } from "react-map-gl/mapbox";
import { CoffeeShopDto } from "../../Data/coffeeshopsDto";
import { useEffect, useRef } from "react";
import { CoffeeShopCard } from "../CoffeeShop/CoffeeShopCards.web";
import { GetDistance } from '../Map/GetDistance'
import { ExternalLocationDto } from "../../Data/ExternalLocationDto";
type Props = {
    stops: location[],
    activeId: string | null,
    setActiveId: (id: string) => void;
}
type location = ExternalLocationDto | CoffeeShopDto;

export function RouteMapView({ stops, activeId, setActiveId }: Props) {
    const token = process.env.MAPBOX_ACCESS_TOKEN;
    const mapRef = useRef<any>(null);
    useEffect(() => {
        if (!activeId) return;
        if (!mapRef.current) return;

        var selected = stops.find((s) => s.id === activeId);
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
    }, [activeId, stops])


    return (
        <div className="routeMapView">
            <Map
                ref={mapRef}
                mapboxAccessToken={token}
                initialViewState={{
                    latitude: 47.6062,
                    longitude: -122.3321,
                    zoom: 10,
                    // TODO CHANGE TO BE IN MIDDLE OF ROUTE and be dependant on route length?
                }}
                mapStyle="mapbox://styles/mapbox/streets-v12"
            >

                {
                    stops.map((s) => (
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
                                className={"pin pin-active"}

                            />
                        </Marker>

                    ))
                }
            </Map>
        </div>
    );
}
