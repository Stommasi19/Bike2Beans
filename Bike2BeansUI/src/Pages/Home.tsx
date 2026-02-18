import { useEffect, useState, useRef } from "react"
import { GetCoffeeShops } from "../Api/coffeeShops"
import { CoffeeShopCard } from "../Features/CoffeeShop/CoffeeShopCards";
import { MapView } from "../Features/Map/MapView";
import { Search } from "../Features/Search/Search";
import { RouteTable } from "../Features/Route/RouteTable";
import { CoffeeShopDto } from "../Data/coffeeshopsDto";
import { RouteDto } from "../Data/RouteDto";

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
    function error(err: any) {
        console.warn(`ERROR(${err.code}): ${err.message}`);
    }
    const options = {
        timeout: 5000,
        maximumAge: 0,
    };
    const [userLocationLat, setUserLocationLat] = useState()
    const [userLocationLng, setUserLocationLng] = useState()
    function success(pos: any) {
        setUserLocationLat(pos.latitude)
        setUserLocationLng(pos.longitude)
    }


    navigator.geolocation.getCurrentPosition(success, error, options)



    const shop1: CoffeeShopDto = {
        id: "697ff291aff3a6744d2cce76",
        name: "McDonald's",
        address: "4 Main St, Maynard, MA 01754, USA",
        rating: 3.4,
        userRatingsTotal: 850,
        lat: 42.43253,
        lng: -71.4500302,
    };

    const shop2: CoffeeShopDto = {

        id: "697ff291aff3a6744d2cce77",
        name: "Boston Bean House",
        address: "102 Main St, Maynard, MA 01754, USA",
        rating: 4.3,
        userRatingsTotal: 211,
        lat: 42.431794,
        lng: -71.45446489999999

    }








    const [routeStops, setRouteStops] = useState<RouteDto[]>([{ stopId: "1", shop: shop1 }]);

    function addShop(shop: CoffeeShopDto) {
        setRouteStops(prev =>
            [...prev,
            { stopId: crypto.randomUUID(), shop }
            ])
    }
    function removeStop(stopId: string) {

        setRouteStops(prev => prev.filter(s => s.stopId !== stopId));


    }
    function reorderStops(next: RouteDto[]) {
        setRouteStops(next);
    }



    return (
        <div className="relative h-screen w-screen" onClick={() => setActiveId(null)}>
            <div className="absolute inset-0">
                {shops ? (<MapView shops={shops} activeId={activeId} setActiveId={setActiveId} />
                ) : (<MapView shops={[]} activeId={"null"} setActiveId={setActiveId} />)}
            </div>
            <div className=" w-80 absolute top-0 inset-x-0">
                <Search />
            </div>
            <div className="route-table">
                <RouteTable
                    routeStops={routeStops}
                    reorderStops={reorderStops}
                    removeStop={removeStop} />
            </div>
            <div className="absolute inset-x-0 bottom-0 z-20 pointer-events-none">
                <div
                    className="w-fit  px-4 pb-4 pointer-events-auto"
                    onClick={(e) => e.stopPropagation()}
                >
                    <div
                        style={{ maxHeight: STACK_MAX_PX }}
                        className="no-scrollbar space-y-2 overflow-y-auto rounded-2xl"
                    >
                        {shops.map((shop) => (

                            <div ref={(node) => {
                                cardRefs.current[shop.id] = node;
                            }}
                                key={shop.id}>
                                <CoffeeShopCard shop={shop} active={shop.id === activeId} onSelect={() => setActiveId(shop.id)} addShop={() => addShop(shop)} /></div>
                        ))}
                    </div>
                </div>
            </div>
        </div>
    )
}
