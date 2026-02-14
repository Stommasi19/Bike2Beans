import React from "react";
import { createRoot } from "react-dom/client";
import { BrowserRouter } from "react-router-dom";
import AppRoutes from "./Navigation/AppRoutes";
import NavBar from "./Components/NavBar";
import 'leaflet/dist/leaflet.css';
import "./styles.css";


createRoot(document.getElementById("root")!).render(
    <BrowserRouter>
        <NavBar />

        <AppRoutes />
    </BrowserRouter>
);
