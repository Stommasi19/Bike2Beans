import { type RouteProp, useRoute } from "@react-navigation/native";
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
import type { RootStackParamList } from "../Navigation/types";

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
    const STACK_MAX_PX = 660;
    const route = useRoute<RouteProp<RootStackParamList, "Home">>();
    const routeEditNotice = route.params?.routeEditNotice;
    const [shops, setShops] = useState<CoffeeshopDto[]>([]);
    const [activeId, setActiveId] = useState<string | null>(null);
    const [routeStops, setRouteStops] = useState<RouteDto[]>(() => route.params?.routeStops ?? []);
    const [routeOptions, setRouteOptions] = useState<RouteOptionDto[]>([]);
    const [selectedRouteId, setSelectedRouteId] = useState<string | null>(null);
    const [userLocation, setUserLocation] = useState<{
        lat: number;
        lng: number;
    } | null>(null);
    const [mapSearchCenter, setMapSearchCenter] = useState<MapSearchCenter | null>(null);
    const [pendingMapSearchCenter, setPendingMapSearchCenter] = useState<MapSearchCenter | null>(null);
    const [isSearchingArea, setIsSearchingArea] = useState(false);
    const [isLoadingShops, setIsLoadingShops] = useState(true);
    const [homeError, setHomeError] = useState<string | null>(null);

    const cardRefs = useRef<Record<string, HTMLDivElement | null>>({});
    const nearbySearchRequestId = useRef(0);

    useEffect(() => {
        if (route.params?.routeStops) {
            setRouteStops(route.params.routeStops);
        }

        if (routeEditNotice) {
            setHomeError(routeEditNotice);
        }
    }, [route.params?.routeStops, routeEditNotice]);

    useEffect(() => {
        GetCoffeeShops()
            .then((nextShops) => {
                setShops(nextShops);
                setHomeError((current) => current === routeEditNotice ? current : null);
            })
            .catch(() => {
                setHomeError("Coffee shops could not be loaded. Search or move the map to try again.");
            })
            .finally(() => setIsLoadingShops(false));
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
                setHomeError(null);
                setPendingMapSearchCenter((current) =>
                    areCentersEqual(current, mapSearchCenter) ? null : current
                );
            } catch (error) {
                if (abortController.signal.aborted || requestId !== nearbySearchRequestId.current) {
                    return;
                }

                setHomeError("This area could not be searched. Please try again.");
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

                setUserLocation({ lat, lng });
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

    function addShop(shop: CoffeeshopDto) {
        setRouteStops((previousStops) => [
            ...previousStops,
            { stopId: crypto.randomUUID(), shop },
        ]);
    }

    const getCoffeeshopFromAutocomplete = async (autocompleteResult: string | null) => {
        if (!autocompleteResult) return;

        try {
            const result = await searchPlacesByText(autocompleteResult);
            const shop = result?.locations?.[0];
            if (!shop) {
                setHomeError("No matching coffee shop was found for that search.");
                return;
            }

            setShops((previousShops) => {
                const isShopInList = previousShops.some(
                    (coffeeshop) => coffeeshop.placeId === shop.placeId
                );

                return isShopInList ? previousShops : [shop, ...previousShops];
            });
            setActiveId(shop.placeId);
            setPendingMapSearchCenter(null);
            setHomeError(null);
        } catch {
            setHomeError("Search failed. Please check your connection and try again.");
        }
    };

    return (
        <div className="map-page" onClick={() => setActiveId(null)}>
            <div className="map-canvas">
                <MapView
                    startLocation={userLocation}
                    shops={shops}
                    activeId={activeId}
                    setActiveId={setActiveId}
                    routeOptions={routeOptions}
                    selectedRouteId={selectedRouteId}
                    onViewportSettled={({ lat, lng }) => {
                        const nextCenter = { lat, lng };
                        if (
                            areCentersEqual(nextCenter, mapSearchCenter) ||
                            areCentersEqual(nextCenter, pendingMapSearchCenter)
                        ) {
                            return;
                        }

                        setPendingMapSearchCenter(nextCenter);
                    }}
                />
            </div>
            <div className="map-top-control-layer" onClick={(event) => event.stopPropagation()}>
                <Search getCoffeeshopFromAutocomplete={getCoffeeshopFromAutocomplete} />
                {pendingMapSearchCenter ? (
                    <button
                        type="button"
                        className="btn-secondary search-area-button"
                        onClick={() => setMapSearchCenter(pendingMapSearchCenter)}
                    >
                        {isSearchingArea ? "Searching..." : "Search this area"}
                    </button>
                ) : null}
                {homeError || isLoadingShops ? (
                    <div className="home-status-panel" role={homeError ? "alert" : "status"}>
                        {homeError ?? "Loading coffee shops..."}
                    </div>
                ) : null}
            </div>
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
            <div className="coffee-card-dock pointer-events-none">
                <div className="coffee-card-stack pointer-events-auto" onClick={(event) => event.stopPropagation()}>
                    <div
                        style={{ maxHeight: STACK_MAX_PX }}
                        className="coffee-card-scroll no-scrollbar"
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
                        {!isLoadingShops && !homeError && shops.length === 0 ? (
                            <div className="empty-card">No coffee shops found nearby.</div>
                        ) : null}
                    </div>
                </div>
            </div>
        </div>
    );
}
