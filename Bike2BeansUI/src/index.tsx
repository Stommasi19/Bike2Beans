import React from "react";
import { createRoot } from "react-dom/client";
import { BrowserRouter } from "react-router-dom";
import AppRoutes from "./Navigation/AppRoutes";
import NavBar from "./Components/NavBar";
import 'leaflet/dist/leaflet.css';


createRoot(document.getElementById("root")!).render(
    <BrowserRouter>
        <NavBar />
        <h1>Hello World</h1>
        <AppRoutes />
    </BrowserRouter>
);
