import React from "react";
import { View } from "react-native";
import { Routes, Route } from "react-router-dom";
import { Home } from "../Pages/Home";
import { Login } from "../Pages/login";

export function AppRoutes() {
    return (
        <View style={{ flex: 1 }}>
            <Routes>
                <Route path="/login" element={<Login />} />
                <Route path="/home" element={<Home />} />
                {/* <Route path="/savedroutes" element={<CoffeeShops />} /> */}
                <Route path="*" element={<Login />} />
            </Routes>
        </View>
    );
}
