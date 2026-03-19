import { CoffeeshopDto } from "../Data/CoffeeshopDto";
import { RouteDto } from "../Data/RouteDto";

export type RootStackParamList = {
    Login: undefined;
    Signup: undefined;
    Home: undefined;
    RouteSetup: { routeStops: RouteDto[] } | undefined;
    SavedRoutes: undefined;
};

