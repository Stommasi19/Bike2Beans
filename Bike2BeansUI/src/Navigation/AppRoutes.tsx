import React from "react";
import { View } from "react-native";
import { Routes, Route } from "react-router-dom";
import { Home } from "../Pages/Home";
import { Login } from "../Pages/Login";
import { RouteSetupManager } from "../Pages/RouteSetupManager";
import { SavedRoutes } from "../Pages/SavedRoutes";
import { Signup } from "../Pages/Signup";
export function AppRoutes() {
    return (
        <View style={{ flex: 1 }}>
            <Routes>
                <Route path="/Login" element={<Login />} />
                <Route path="/Signup" element={<Signup />} />

                <Route path="/Home" element={<Home />} />
                <Route path="/RouteSetup" element={<RouteSetupManager />} />
                <Route path="/SavedRoutes" element={<SavedRoutes />} />

                <Route path="*" element={<Login />} />
            </Routes>
        </View>
    );
}
