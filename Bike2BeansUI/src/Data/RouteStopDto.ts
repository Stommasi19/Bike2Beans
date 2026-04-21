export enum RouteStopLocationType {
    Coffeeshop = 0,
    Landmark = 1,
    Other = 2,
}

export type RouteStopDto = {
    id: string;
    placeId: string;
    name: string;
    address: string;
    locationType: RouteStopLocationType;
    lat: number;
    lng: number;
};
