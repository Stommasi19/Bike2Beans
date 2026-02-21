import React from "react";
import { createNativeStackNavigator } from "@react-navigation/native-stack";
import { Home } from "../Pages/Home.native";
import { Login } from "../Pages/Login.native";
import { RouteSetupManager } from "../Pages/RouteSetupManager.native";
import { SavedRoutes } from "../Pages/SavedRoutes.native";
import { Signup } from "../Pages/Signup.native";
import type { RootStackParamList } from "./types";

const Stack = createNativeStackNavigator<RootStackParamList>();

export function AppRoutes() {
    return (
        <Stack.Navigator initialRouteName="Login" screenOptions={{ headerShown: false }}>
            <Stack.Screen name="Login" component={Login} />
            <Stack.Screen name="Signup" component={Signup} />
            <Stack.Screen name="Home" component={Home} />
            <Stack.Screen name="RouteSetup" component={RouteSetupManager} />
            <Stack.Screen name="SavedRoutes" component={SavedRoutes} />
        </Stack.Navigator>
    );
}
