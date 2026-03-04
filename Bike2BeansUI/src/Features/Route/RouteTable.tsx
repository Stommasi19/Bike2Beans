import { LocationBox } from "./LocationBox"
import { CoffeeshopDto } from "../../Data/CoffeeshopDto"
import { RouteDto } from "../../Data/RouteDto"
import { useNavigation } from "@react-navigation/native";
import { RootStackParamList } from "../../Navigation/types";
import { NativeStackNavigationProp } from "@react-navigation/native-stack";

type Props = {
    routeStops: RouteDto[];
    reorderStops: (shops: RouteDto[]) => void;
    removeStop: (stopId: string) => void;
    openRouteSetup: () => void;

}

export function RouteTable({ routeStops, reorderStops, removeStop, openRouteSetup }: Props) {
    return (
        <div className="route-container center">
            <h1 className="element-header">
                Add Shops To A Route            </h1>
            <LocationBox routeStops={routeStops} reorderStops={reorderStops} removeStop={removeStop} />
            {routeStops.length > 0 && (

                <button className="btn-primary center"
                    onClick={openRouteSetup}>Route Creation Mode</button>
            )}
        </div>
    )
}