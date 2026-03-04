import React from "react";
import { createRoot } from "react-dom/client";
import { AppRoutes } from "./Navigation/AppRoutes.web";
import { NavigationContainer } from "@react-navigation/native";
import { NavBar } from "./Components/NavBar.web";
import "./styles.css";
import 'mapbox-gl/dist/mapbox-gl.css';
import type { RootStackParamList } from "./Navigation/types";

const linking = {
    prefixes: [window.location.origin],
    config: {
        screens: {
            Login: "login",
            Signup: "signup",
            Home: "home",
            RouteSetup: "route-setup",
            SavedRoutes: "saved-routes",
        } satisfies Record<keyof RootStackParamList, string>,
    },
};

if (window.location.pathname === "/") {
    window.history.replaceState(null, "", "/login");
}
createRoot(document.getElementById("root")!).render(
    <NavigationContainer linking={linking}>
        <div style={{ display: "flex", flexDirection: "column", height: "100vh" }}>
            <NavBar />
            <div style={{ flex: 1, position: "relative", overflow: "hidden" }}>
                <AppRoutes />
            </div>
        </div>
    </NavigationContainer>
);

