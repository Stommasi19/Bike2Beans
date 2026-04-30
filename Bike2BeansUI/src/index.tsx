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

function WebAppShell() {
    const [pathname, setPathname] = React.useState(window.location.pathname);
    const isAuthRoute = pathname === "/login" || pathname === "/signup";

    React.useEffect(() => {
        const syncPathname = () => setPathname(window.location.pathname);
        window.addEventListener("popstate", syncPathname);

        return () => {
            window.removeEventListener("popstate", syncPathname);
        };
    }, []);

    return (
        <div className="app-shell">
            {!isAuthRoute ? <NavBar /> : null}
            <main className="app-content">
                <AppRoutes />
            </main>
        </div>
    );
}

createRoot(document.getElementById("root")!).render(
    <NavigationContainer linking={linking}>
        <WebAppShell />
    </NavigationContainer>
);
