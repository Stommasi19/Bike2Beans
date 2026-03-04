import { CoffeeshopDto } from "./CoffeeshopDto";
import { ExternalLocationDto } from "./ExternalLocationDto";


export type RouteDto = {
    stopId: string;
    shop: CoffeeshopDto | ExternalLocationDto;
}