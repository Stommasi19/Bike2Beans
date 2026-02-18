import React from "react";
import { createRoot } from "react-dom/client";
import { BrowserRouter } from "react-router-dom";
import { AppRoutes } from "./Navigation/AppRoutes";
import { NavBar } from "./components/NavBar";
import "./styles.css";
import 'mapbox-gl/dist/mapbox-gl.css';


createRoot(document.getElementById("root")!).render(
    <BrowserRouter>
        <NavBar />

        <AppRoutes />
    </BrowserRouter>
);
