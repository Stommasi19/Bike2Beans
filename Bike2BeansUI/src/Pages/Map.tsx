import React from 'react';
import { MapContainer, TileLayer, Marker, Popup } from 'react-leaflet';
import 'leaflet/dist/leaflet.css';
import type { LatLngExpression } from "leaflet";
import "leaflet/dist/leaflet.css";



const Map = () => {

    const center: LatLngExpression = [42.3555, -71.0565];
    const zoomLevel = 13;

    return (
        // The map container must have a fixed height and width defined in CSS for visibility
        <MapContainer center={center} zoom={zoomLevel} scrollWheelZoom={false} style={{ height: '500px', width: '80%' }}>

            {/* Add the OpenStreetMap tile layer */}
            <TileLayer
                attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
                url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
            />

        </MapContainer>
    );
}
export default Map;