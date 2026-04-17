import { useEffect, useState, useRef } from "react"
import { GetCoffeeShops } from "../Api/Coffeeshops"
import { CoffeeShopCard } from "../Features/CoffeeShop/CoffeeShopCards.web";
import { MapView } from "../Features/Map/MapView";
import { Search } from "../Features/Search/Search";
import { CoffeeshopDto } from "../Data/CoffeeshopDto";
import { RouteDto } from "../Data/RouteDto";
import { RouteOptionDto } from "../Data/RouteOptionDto";
import { RouteSetupManager } from "./RouteSetupManager.web";
import { searchPlacesByText, searchPlacesNearby } from "../Api/Places";

export function Home() {
    const STACK_MAX_PX = 660
    const [shops, setShops] = useState<any[]>([])
    useEffect(() => {
        GetCoffeeShops()
            .then(setShops)
            .catch(console.error);



    }, []);
    const [activeId, setActiveId] = useState<string | null>(null);
    const cardRefs = useRef<Record<string, HTMLDivElement | null>>({});
    useEffect(() => {
        if (!activeId) return;
        const selectedShop = cardRefs.current[activeId];

        selectedShop?.scrollIntoView({
            behavior: "smooth",
            block: "nearest",
            inline: "nearest"
        })
    }, [activeId])
    const [routeStops, setRouteStops] = useState<RouteDto[]>([]);
    const suppressNextMove = useRef(false);


    function addShop(shop: CoffeeshopDto) {
        setRouteStops(prev =>
            [...prev,
            { stopId: crypto.randomUUID(), shop }
            ])
    }


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

    useEffect(() => {
        if (!mapSearchCenter) return;
        console.log("mapSearchCenter changed", mapSearchCenter);

        const timeoutId = window.setTimeout(async () => {
            try {
                console.log("about to call nearby API", mapSearchCenter);
                const nearby = await searchPlacesNearby(
                    mapSearchCenter.lat,
                    mapSearchCenter.lng
                );
                console.log("nearby response", nearby);
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

                setUserLocation({
                    lat,
                    lng,
                });
                await searchPlacesNearby(lat, lng).then((nearby) => {
                    console.log("nearby response", nearby);
                    setShops(nearby);
                })
                    .catch((error) => {
                        console.warn("nearby fetch failed", error);
                    });
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

    const getCoffeeshopFromAutocomplete = async (autocompleteResult: any) => {
        const result = await searchPlacesByText(autocompleteResult)
        const shop = result.locations[0]
        console.log(shop)
        setShops((prev: CoffeeshopDto[]) => {
            const isShopInList = prev.some((coffeeshop: CoffeeshopDto) => coffeeshop.placeId === shop.placeId)
            return isShopInList ? prev : [shop, ...prev]
        });
        setActiveId(shop.placeId)
        suppressNextMove.current = false
    }

    return (
        <div className="absolute h-full w-full" onClick={() => setActiveId(null)}>
            <div className="absolute inset-0">
                {shops ? (<MapView
                    startLocation={userLocation}
                    shops={shops}
                    activeId={activeId}
                    setActiveId={setActiveId}
                    routeOptions={routeOptions}
                    selectedRouteId={selectedRouteId}
                    onViewportSettled={({ lat, lng, zoom }) => {
                        console.log("viewport settled", lat, lng, zoom);
                        setMapSearchCenter({ lat, lng });
                    }}
                />
                ) : (<MapView
                    shops={[]}
                    activeId={activeId}
                    setActiveId={setActiveId}
                    routeOptions={routeOptions}
                    selectedRouteId={selectedRouteId}
                    onViewportSettled={({ lat, lng, zoom }) => {
                        console.log("viewport settled", lat, lng, zoom);
                        setMapSearchCenter({ lat, lng });
                    }} />)}
            </div>
            <div className=" absolute top-0 inset-x-0">
                <Search
                    getCoffeeshopFromAutocomplete={getCoffeeshopFromAutocomplete}
                />
            </div>
            <div className="route-table-container">
                {routeStops.length > 0 && (
                    <RouteSetupManager
                        routeStops={routeStops}
                        setRouteStops={setRouteStops}
                        routeOptions={routeOptions}
                        setRouteOptions={setRouteOptions}
                        selectedRouteId={selectedRouteId}
                        setSelectedRouteId={setSelectedRouteId} />
                )}

            </div>
            <div className="fixed bottom-0 inset-x-0 z-20 pointer-events-none">

                <div
                    className="w-fit   pointer-events-auto"
                    onClick={(e) => e.stopPropagation()}
                >

                    <div
                        style={{ maxHeight: STACK_MAX_PX }}
                        className="no-scrollbar space-y-2 overflow-y-auto rounded-2xl"
                    >

                        {shops.map((shop) => (

                            <div ref={(node) => {
                                cardRefs.current[shop.placeId] = node;
                            }}
                                key={shop.placeId}>
                                <CoffeeShopCard shop={shop} active={shop.placeId === activeId} onSelect={() => setActiveId(shop.placeId)} addShop={() => addShop(shop)} />
                            </div>
                        ))}
                    </div>
                </div>
            </div>
        </div>
    )
}
