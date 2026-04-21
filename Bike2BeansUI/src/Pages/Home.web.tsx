import { useEffect, useRef, useState } from "react"
import { GetCoffeeShops } from "../Api/coffeeShops"
import { CoffeeShopCard } from "../Features/CoffeeShop/CoffeeShopCards.web";
import { MapView } from "../Features/Map/MapView";
import { Search } from "../Features/Search/Search";
import { CoffeeshopDto } from "../Data/CoffeeshopDto";
import { RouteDto } from "../Data/RouteDto";
import { RouteOptionDto } from "../Data/RouteOptionDto";
import { RouteSetupManager } from "./RouteSetupManager.web";
import { searchPlacesByText, searchPlacesNearby } from "../Api/places";

type MapSearchCenter = {
    lat: number;
    lng: number;
};

function areCentersEqual(left: MapSearchCenter | null, right: MapSearchCenter | null) {
    if (!left || !right) {
        return false;
    }

    return left.lat === right.lat && left.lng === right.lng;
}

export function Home() {
    const STACK_MAX_PX = 660
    const [shops, setShops] = useState<CoffeeshopDto[]>([])
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

    const [mapSearchCenter, setMapSearchCenter] = useState<MapSearchCenter | null>(null);
    const [pendingMapSearchCenter, setPendingMapSearchCenter] = useState<MapSearchCenter | null>(null);
    const [isSearchingArea, setIsSearchingArea] = useState(false);
    const nearbySearchRequestId = useRef(0);

    useEffect(() => {
        if (!mapSearchCenter) return;

        const requestId = ++nearbySearchRequestId.current;
        const abortController = new AbortController();
        setIsSearchingArea(true);

        void (async () => {
            try {
                const nearby = await searchPlacesNearby(
                    mapSearchCenter.lat,
                    mapSearchCenter.lng,
                    { signal: abortController.signal }
                );

                if (abortController.signal.aborted || requestId !== nearbySearchRequestId.current) {
                    return;
                }

                setShops(nearby);
                setPendingMapSearchCenter((current) =>
                    areCentersEqual(current, mapSearchCenter) ? null : current
                );
            } catch (error) {
                if (abortController.signal.aborted || requestId !== nearbySearchRequestId.current) {
                    return;
                }

                console.warn("nearby fetch failed", error);
            } finally {
                if (!abortController.signal.aborted && requestId === nearbySearchRequestId.current) {
                    setIsSearchingArea(false);
                }
            }
        })();

        return () => {
            abortController.abort();
        };
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
                setMapSearchCenter({ lat, lng });
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

        if (!shop) {
            return;
        }

        setShops((prev: CoffeeshopDto[]) => {
            const isShopInList = prev.some((coffeeshop: CoffeeshopDto) => coffeeshop.placeId === shop.placeId)
            return isShopInList ? prev : [shop, ...prev]
        });
        setActiveId(shop.placeId)
        setPendingMapSearchCenter(null)
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
                    onViewportSettled={({ lat, lng }) => {
                        const nextCenter = { lat, lng };
                        if (areCentersEqual(nextCenter, mapSearchCenter) || areCentersEqual(nextCenter, pendingMapSearchCenter)) {
                            return;
                        }

                        setPendingMapSearchCenter(nextCenter);
                    }}
                />
                ) : (<MapView
                    shops={[]}
                    activeId={activeId}
                    setActiveId={setActiveId}
                    routeOptions={routeOptions}
                    selectedRouteId={selectedRouteId}
                    onViewportSettled={({ lat, lng }) => {
                        const nextCenter = { lat, lng };
                        if (areCentersEqual(nextCenter, mapSearchCenter) || areCentersEqual(nextCenter, pendingMapSearchCenter)) {
                            return;
                        }

                        setPendingMapSearchCenter(nextCenter);
                    }} />)}
            </div>
            {pendingMapSearchCenter ? (
                <div className="absolute right-4 top-24 z-20">
                    <button
                        type="button"
                        className="btn-secondary"
                        onClick={() => setMapSearchCenter(pendingMapSearchCenter)}
                    >
                        {isSearchingArea ? "Searching..." : "Search this area"}
                    </button>
                </div>
            ) : null}
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
