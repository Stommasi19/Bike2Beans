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
        <div className="route-container center">
            <h1 className="element-header">
                Add Shops To A Route            </h1>
            <LocationBox routeStops={routeStops} reorderStops={reorderStops} removeStop={removeStop} />
            {routeStops.length > 0 && (
                <button className="btn-primary center"> Route Creation Mode</button>
            )}
        </div>
    )
}