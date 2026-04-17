import { type RouteProp, useRoute } from "@react-navigation/native";
import { createNativeStackNavigator } from "@react-navigation/native-stack";
import { useState } from "react";
import type { RouteDto } from "../Data/RouteDto";
import type { RouteOptionDto } from "../Data/RouteOptionDto";
import { Home } from "../Pages/Home.web";
import { Login } from "../Pages/Login.web";
import { RouteSetupManager } from "../Pages/RouteSetupManager.web";
import { SavedRoutes } from "../Pages/SavedRoutes.web";
import { Signup } from "../Pages/Signup.web";
import type { RootStackParamList } from "./types";

const Stack = createNativeStackNavigator<RootStackParamList>();

function RouteSetupScreen() {
    const route = useRoute<RouteProp<RootStackParamList, "RouteSetup">>();
    const [routeStops, setRouteStops] = useState<RouteDto[]>(() => route.params?.routeStops ?? []);
    const [routeOptions, setRouteOptions] = useState<RouteOptionDto[]>([]);
    const [selectedRouteId, setSelectedRouteId] = useState<string | null>(null);

    return (
        <RouteSetupManager
            routeStops={routeStops}
            setRouteStops={setRouteStops}
            routeOptions={routeOptions}
            setRouteOptions={setRouteOptions}
            selectedRouteId={selectedRouteId}
            setSelectedRouteId={setSelectedRouteId}
        />
    );
}

export function AppRoutes() {
    return (
        <Stack.Navigator initialRouteName="Login" screenOptions={{ headerShown: false }}>
            <Stack.Screen name="Login" component={Login} />
            <Stack.Screen name="Signup" component={Signup} />
            <Stack.Screen name="Home" component={Home} />
            <Stack.Screen name="RouteSetup" component={RouteSetupScreen} />
            <Stack.Screen name="SavedRoutes" component={SavedRoutes} />
        </Stack.Navigator>
    );
}
