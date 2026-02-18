import { CoffeeShopDto } from "../../Data/coffeeshopsDto";
import { RouteDto } from "../../Data/RouteDto";
type Props = {
    shop: CoffeeShopDto;
    active?: boolean | string;
    onSelect?: () => void;
    addShop?: (shop: CoffeeShopDto) => void;
    removeStop?: (stopId: string) => void;
    stopId?: string;
};

export function CoffeeShopCard({ shop, active = false, onSelect, addShop, removeStop, stopId }: Props) {

    return (
        <div
            onClick={onSelect}
            className={"coffeeShopCard"}
            data-state={active}
        >

            <div
                className={"coffeeShopCardTitle"}
                data-state={active}
            >
                {shop.name}
            </div>


            <div className="coffeeShopCardRating" data-state={active}>
                <span className="coffeeShopCardRating" data-state={active}>
                    ★ {shop.rating ?? "—"}
                </span>
                <span className="opacity-70 text-gray-600">
                    ({shop.userRatingsTotal ?? 0})
                </span>
            </div>
            {active === true && (
                <div className="coffeeShopCardAdd">
                    <button className="btn-primary"
                        onClick={() => addShop?.(shop)}>Add To Route</button>
                </div>
            )}

            {active == "route" && (
                <div className="coffeeShopCardDelete">
                    <button className="btn-secondary"
                        onClick={() => { if (stopId) removeStop?.(stopId) }}>Remove</button>
                </div>
            )}




            <div className="coffeeShopCardDivider" data-state={active} />


            <div className="coffeeShopCardRemainder" data-state={active}>
                {shop.address}
            </div>
        </div>
    );
}
