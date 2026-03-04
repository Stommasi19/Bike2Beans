import { useEffect, useState, useRef } from "react"
import { GetCoffeeShops } from "../Api/coffeeShops"
import { CoffeeShopCard } from "../Features/CoffeeShop/CoffeeShopCards.web";
import { MapView } from "../Features/Map/MapView";
import { Search } from "../Features/Search/Search";
import { RouteTable } from "../Features/Route/RouteTable";
import { CoffeeshopDto } from "../Data/CoffeeshopDto";
import { RouteDto } from "../Data/RouteDto";
import { useNavigation } from "@react-navigation/native";
import { NativeStackNavigationProp } from "@react-navigation/native-stack";
import { RootStackParamList } from "../Navigation/types";

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



    const nav = useNavigation<NativeStackNavigationProp<RootStackParamList>>();

    function openRouteSetup() {
        nav.navigate("RouteSetup", { routeStops });
    }






    const [routeStops, setRouteStops] = useState<RouteDto[]>([]);

    function addShop(shop: CoffeeshopDto) {
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

    console.log("shops: ", shops)

    return (
        <div className="absolute h-full w-full" onClick={() => setActiveId(null)}>
            <div className="absolute inset-0">
                {shops ? (<MapView shops={shops} activeId={activeId} setActiveId={setActiveId} />
                ) : (<MapView shops={[]} activeId={"null"} setActiveId={setActiveId} />)}
            </div>
            <div className=" absolute top-0 inset-x-0">
                <Search />
            </div>
            <div className="route-table-container">
                <RouteTable
                    routeStops={routeStops}
                    reorderStops={reorderStops}
                    removeStop={removeStop}
                    openRouteSetup={openRouteSetup} />
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
                                cardRefs.current[shop.id] = node;
                            }}
                                key={shop.id}>
                                <CoffeeShopCard shop={shop} active={shop.id === activeId} onSelect={() => setActiveId(shop.id)} addShop={() => addShop(shop)} />
                            </div>
                        ))}
                    </div>
                </div>
            </div>
        </div>
    )
}
