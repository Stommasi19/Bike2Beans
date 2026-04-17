import { RefObject } from "react"
import { GetDistance } from "./GetDistance"


type Props = {
    map: mapboxgl.Map,
    center: { lat: number, lng: number },
    selected: { lat: number, lng: number }

}
export function MapMover(map: mapboxgl.Map, center: { lat: number, lng: number }, selected: { lat: number, lng: number }) {
    const distance = GetDistance(center.lat, center.lng, selected.lat, selected.lng)

    if (distance < 50) {
        map.easeTo({
            center: [selected.lng, selected.lat],
            zoom: 14,
            duration: 800,
        });
    } else {
        map.flyTo({
            center: [selected.lng, selected.lat],
            zoom: 14,
            duration: 8000,
        });
    }
}