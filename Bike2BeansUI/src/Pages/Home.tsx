import { useEffect, useState } from "react"
import { GetCoffeeShops } from "../Api/coffeeShops"
import { CoffeeShopCard } from "../Features/CoffeeShop/CoffeeShopCards";
import { MapView } from "../Features/Map/MapView";
import { Search } from "../Features/Search/Search";

export function Home() {
    const STACK_MAX_PX = 660
    const [shops, setShops] = useState<any[]>([])
    useEffect(() => {
        GetCoffeeShops()
            .then(setShops)
            .catch(console.error);
    }, []);
    const [activeId, setActiveId] = useState<string | null>(null);

    const selectedShop = shops.find((s) => s.id === activeId)
    return (
        <div className="relative h-screen w-screen" onClick={() => setActiveId(null)}>
            <div className="absolute inset-0">
                {/* <MapView /> */}
            </div>
            <div className=" w-80 absolute top-0 inset-x-0">
                <Search />
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

                            <div key={shop.id}><CoffeeShopCard shop={shop} active={shop.id === activeId} onSelect={() => setActiveId(shop.id)} /></div>
                        ))}
                    </div>
                </div>
            </div>
        </div>
    )
}
