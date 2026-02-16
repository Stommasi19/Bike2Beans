import Map, { Marker } from "react-map-gl/mapbox";
import { CoffeeShopDto } from "../../Data/coffeeshopsDto";
import { useEffect, useRef } from "react";
import { CoffeeShopCard } from "../CoffeeShop/CoffeeShopCards";
type Props = {
    shops: CoffeeShopDto[],
    activeId: string | null,
    setActiveId: (id: string) => void;
}


export function MapView({ shops, activeId, setActiveId }: Props) {
    const token = process.env.MAPBOX_ACCESS_TOKEN;
    const mapRef = useRef<any>(null);
    useEffect(() => {
        if (!activeId) return;
        if (!mapRef.current) return;

        var selected = shops.find((s) => s.id === activeId);
        if (!selected) return;

        const map = mapRef.current.getMap();

        map.easeTo({
            center: [selected.lng, selected.lat],
            zoom: 14,
            duration: 800,
        })
    }, [activeId])


    return (
        <div style={{ height: "100vh", width: "100vw" }}>
            <Map
                ref={mapRef}
                mapboxAccessToken={token}
                initialViewState={{
                    latitude: 47.6062,
                    longitude: -122.3321,
                    zoom: 10,
                }}
                mapStyle="mapbox://styles/mapbox/streets-v12"
            >

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
            </Map>
        </div>
    );
}
