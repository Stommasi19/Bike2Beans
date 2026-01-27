import React from "react";
import { View } from "react-native";
import { Routes, Route } from "react-router-dom";

import CoffeeShops from "../Pages/CoffeeShops";
import Map from "../Pages/Map";
import Login from "../Pages/login";

export default function AppRoutes() {
    return (
        <View style={{ flex: 1 }}>
            <Routes>
                <Route path="/login" element={<Login />} />
                <Route path="/map" element={<Map />} />
                <Route path="/coffeeshop" element={<CoffeeShops />} />
                <Route path="*" element={<Login />} />
            </Routes>
        </View>
    );
}
