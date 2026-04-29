import type { CoffeeshopDto } from "../../Data/CoffeeshopDto";
import type { ExternalLocationDto } from "../../Data/ExternalLocationDto";

type Props = {
    shop: CoffeeshopDto | ExternalLocationDto;
    active?: boolean | string;
    onSelect?: () => void;
    addShop?: (shop: CoffeeshopDto) => void;
    removeStop?: (stopId: string) => void;
    stopId?: string;
};

export function CoffeeShopCard({ shop, active = false, onSelect, addShop, removeStop, stopId }: Props) {
    const isRouteCard = active === "route";
    const isCoffeeshop = "placeId" in shop;
    const canSelectCard = !isRouteCard && active !== true;
    const CardShell = canSelectCard ? "button" : "div";

    return (
        <CardShell
            {...(canSelectCard
                ? {
                    type: "button" as const,
                    onClick: onSelect,
                }
                : {})}
            className={"coffeeShopCard z-9999"}
            data-state={active}
            aria-label={canSelectCard ? `Show details for ${shop.name}` : undefined}
        >
            <div className="coffeeShopCardHead" data-state={active}>
                <div className="coffeeShopCardMeta">
                    <div
                        className={"coffeeShopCardTitle"}
                        data-state={active}
                        title={shop.name}
                    >
                        {shop.name}
                    </div>


                    <div className="coffeeShopCardRating" data-state={active}>
                        <span className="coffeeShopCardRating" data-state={active}>
                            ★ {isCoffeeshop ? shop.rating ?? "—" : "—"}
                        </span>
                        <span className="opacity-70 text-gray-600">
                            ({isCoffeeshop ? shop.userRatingsTotal ?? 0 : 0})
                        </span>
                    </div>
                </div>
                {isRouteCard ? (
                    <button
                        type="button"
                        className="btn-secondary coffeeShopCardInlineAction"
                        onClick={(event) => {
                            event.stopPropagation();
                            if (stopId) removeStop?.(stopId);
                        }}
                    >
                        Remove
                    </button>
                ) : null}
            </div>
            {active === true && (
                <div className="coffeeShopCardAdd">
                    <button
                        type="button"
                        className="btn-primary"
                        onClick={(event) => {
                            event.stopPropagation();
                            if (isCoffeeshop) {
                                addShop?.(shop);
                            }
                        }}
                    >
                        Add To Route
                    </button>
                </div>
            )}

            <div className="coffeeShopCardDivider" data-state={active} />


            <div className="coffeeShopCardRemainder" data-state={active}>
                {shop.address || "Address unavailable"}
            </div>
        </CardShell>
    );
}
