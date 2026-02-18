import { LocationBox } from "./LocationBox"
import { CoffeeShopDto } from "../../Data/coffeeshopsDto"
import { RouteDto } from "../../Data/RouteDto"

type Props = {
    routeStops: RouteDto[];
    reorderStops: (shops: RouteDto[]) => void;
    removeStop: (stopId: string) => void
}
export function RouteTable({ routeStops, reorderStops, removeStop }: Props) {

    return (
        <div className="route-table">
            <div>
                Create a Route
            </div>
            <LocationBox routeStops={routeStops} reorderStops={reorderStops} removeStop={removeStop} />
        </div>
    )
}