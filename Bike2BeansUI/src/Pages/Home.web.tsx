import { useEffect, useRef, useState } from "react";
import { GetCoffeeShops } from "../Api/coffeeShops";
import { searchPlacesByText, searchPlacesNearby } from "../Api/places";
import { CoffeeShopCard } from "../Features/CoffeeShop/CoffeeShopCards.web";
import { MapView } from "../Features/Map/MapView";
import { Search } from "../Features/Search/Search";
import type { CoffeeshopDto } from "../Data/CoffeeshopDto";
import type { RouteDto } from "../Data/RouteDto";
import type { RouteOptionDto } from "../Data/RouteOptionDto";
import { RouteSetupManager } from "./RouteSetupManager.web";

export function Home() {
    const STACK_MAX_PX = 660;
    const [shops, setShops] = useState<CoffeeshopDto[]>([]);
    const [activeId, setActiveId] = useState<string | null>(null);
    const [routeStops, setRouteStops] = useState<RouteDto[]>([]);
    const [routeOptions, setRouteOptions] = useState<RouteOptionDto[]>([]);
    const [selectedRouteId, setSelectedRouteId] = useState<string | null>(null);
    const [userLocation, setUserLocation] = useState<{
        lat: number;
        lng: number;
    } | null>(null);
    const [mapSearchCenter, setMapSearchCenter] = useState<{
        lat: number;
        lng: number;
    } | null>(null);

    const cardRefs = useRef<Record<string, HTMLDivElement | null>>({});

    useEffect(() => {
        GetCoffeeShops()
            .then(setShops)
            .catch(console.error);
    }, []);

    useEffect(() => {
        if (!activeId) return;

        const selectedShop = cardRefs.current[activeId];
        selectedShop?.scrollIntoView({
            behavior: "smooth",
            block: "nearest",
            inline: "nearest",
        });
    }, [activeId]);

    useEffect(() => {
        if (!mapSearchCenter) return;

        const timeoutId = window.setTimeout(async () => {
            try {
                const nearby = await searchPlacesNearby(
                    mapSearchCenter.lat,
                    mapSearchCenter.lng
                );
                setShops(nearby);
            } catch (error) {
                console.warn("nearby fetch failed", error);
            }
        }, 2000);

        return () => window.clearTimeout(timeoutId);
    }, [mapSearchCenter]);

    useEffect(() => {
        if (typeof navigator === "undefined" || !navigator.geolocation) {
            console.warn("Geolocation is not available in this browser context.");
            return;
        }

        navigator.geolocation.getCurrentPosition(
            async ({ coords }) => {
                const lat = Number(coords.latitude.toFixed(3));
                const lng = Number(coords.longitude.toFixed(3));

                setUserLocation({ lat, lng });

                try {
                    const nearby = await searchPlacesNearby(lat, lng);
                    setShops(nearby);
                } catch (error) {
                    console.warn("nearby fetch failed", error);
                }
            },
            (error) => {
                console.warn(error);
            },
            {
                enableHighAccuracy: false,
                timeout: 5000,
                maximumAge: 300000,
            }
        );
    }, []);

    function addShop(shop: CoffeeshopDto) {
        setRouteStops((previousStops) => [
            ...previousStops,
            { stopId: crypto.randomUUID(), shop },
        ]);
    }

    const getCoffeeshopFromAutocomplete = async (autocompleteResult: string | null) => {
        if (!autocompleteResult) return;

        const result = await searchPlacesByText(autocompleteResult);
        const shop = result?.locations?.[0];
        if (!shop) return;

        setShops((previousShops) => {
            const isShopInList = previousShops.some(
                (coffeeshop) => coffeeshop.placeId === shop.placeId
            );

            return isShopInList ? previousShops : [shop, ...previousShops];
        });
        setActiveId(shop.placeId);
    };

    return (
        <div className="absolute h-full w-full" onClick={() => setActiveId(null)}>
            <div className="absolute inset-0">
                <MapView
                    startLocation={userLocation}
                    shops={shops}
                    activeId={activeId}
                    setActiveId={setActiveId}
                    routeOptions={routeOptions}
                    selectedRouteId={selectedRouteId}
                    onViewportSettled={({ lat, lng }) => {
                        setMapSearchCenter({ lat, lng });
                    }}
                />
            </div>
            <div className="absolute top-0 inset-x-0">
                <Search getCoffeeshopFromAutocomplete={getCoffeeshopFromAutocomplete} />
            </div>
            <div className="route-table-container">
                {routeStops.length > 0 ? (
                    <RouteSetupManager
                        routeStops={routeStops}
                        setRouteStops={setRouteStops}
                        routeOptions={routeOptions}
                        setRouteOptions={setRouteOptions}
                        selectedRouteId={selectedRouteId}
                        setSelectedRouteId={setSelectedRouteId}
                    />
                ) : null}
            </div>
            <div className="fixed bottom-0 inset-x-0 z-20 pointer-events-none">
                <div
                    className="w-fit pointer-events-auto"
                    onClick={(event) => event.stopPropagation()}
                >
                    <div
                        style={{ maxHeight: STACK_MAX_PX }}
                        className="no-scrollbar space-y-2 overflow-y-auto rounded-2xl"
                    >
                        {shops.map((shop) => (
                            <div
                                ref={(node) => {
                                    cardRefs.current[shop.placeId] = node;
                                }}
                                key={shop.placeId}
                            >
                                <CoffeeShopCard
                                    shop={shop}
                                    active={shop.placeId === activeId}
                                    onSelect={() => setActiveId(shop.placeId)}
                                    addShop={() => addShop(shop)}
                                />
                            </div>
                        ))}
                    </div>
                </div>
            </div>
        </div>
    );
}
