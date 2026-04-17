import { CoffeeshopDto } from './CoffeeshopDto'


export type RouteDetailsDto = {
    Id: string;
    Name: string;
    StartLocation: number;
    EndLocation: number;
    RouteStops: [CoffeeshopDto];
    Mileage: number;
}
