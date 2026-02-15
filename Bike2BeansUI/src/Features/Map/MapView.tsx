import Map from "react-map-gl/mapbox";


export function MapView() {
    const token = process.env.MAPBOX_ACCESS_TOKEN;

    return (
        <div style={{ height: "100vh", width: "100vw" }}>
            <Map
                mapboxAccessToken={token}
                initialViewState={{
                    latitude: 47.6062,
                    longitude: -122.3321,
                    zoom: 10,
                }}
                mapStyle="mapbox://styles/mapbox/streets-v12"
            />
        </div>
    );
}
