import { CoffeeShopDto } from "../../Data/coffeeshopsDto";
type Props = {
    shop: CoffeeShopDto;
    active?: boolean;
    onSelect: () => void;
};

export function CoffeeShopCard({ shop, active = false, onSelect }: Props) {
    return (
        <div
            onClick={onSelect}
            className={[
                "rounded-xl border transition-all duration-200",
                "px-4 py-3",
                active
                    ? "w-96 h-40 bg-latte-50 border-latte-400"
                    : "w-80 bg-white border-gray-200 hover:border-latte-200"
            ].join(" ")}
        >
            {/* Title */}
            <div
                className={[
                    "text-base font-semibold tracking-tight",
                    active ? "text-latte-600" : "text-gray-900"
                ].join(" ")}
            >
                {shop.name}
            </div>

            {/* Rating */}
            <div className="mt-1 flex items-center gap-2 text-sm text-gray-600">
                <span className={active ? "text-latte-500 font-medium" : ""}>
                    ★ {shop.rating ?? "—"}
                </span>
                <span className="opacity-70">
                    ({shop.user_rating_total ?? 0})
                </span>
            </div>

            {/* Subtle divider */}
            <div className="mt-3 h-px bg-gray-100" />

            {/* Footer row */}
            <div className="mt-2 text-xs uppercase tracking-wide text-gray-500">
                {shop.lat.toFixed(3)}, {shop.lng.toFixed(3)}
            </div>
        </div>
    );
}
