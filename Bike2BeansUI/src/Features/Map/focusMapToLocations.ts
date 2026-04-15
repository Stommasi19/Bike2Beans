// import type { Map as MapboxMap } from "mapbox-gl";
// import { GetDistance } from "./GetDistance";

// export type MapLocation = {
//     lat: number;
//     lng: number;
// };

// type FocusOptions = {
//     padding?: number;
//     maxZoom?: number;
//     minZoom?: number;
//     singlePointZoom?: number;
//     durationMs?: number;
//     distanceThresholdKm?: number;
//     zoomThreshold?: number;
// };

// type CenterAndZoom = {
//     center: { lng: number; lat: number };
//     zoom: number;
// };

// type InitialViewState = {
//     latitude: number;
//     longitude: number;
//     zoom: number;
// };

// function buildBounds(locations: MapLocation[]): [[number, number], [number, number]] | null {
//     if (locations.length === 0) return null;

//     let minLng = locations[0].lng;
//     let maxLng = locations[0].lng;
//     let minLat = locations[0].lat;
//     let maxLat = locations[0].lat;

//     for (const location of locations) {
//         minLng = Math.min(minLng, location.lng);
//         maxLng = Math.max(maxLng, location.lng);
//         minLat = Math.min(minLat, location.lat);
//         maxLat = Math.max(maxLat, location.lat);
//     }

//     return [
//         [minLng, minLat],
//         [maxLng, maxLat],
//     ];
// }

// function clamp(value: number, min: number, max: number): number {
//     return Math.min(Math.max(value, min), max);
// }

// function latToMercatorYFraction(lat: number): number {
//     const sin = Math.sin((lat * Math.PI) / 180);
//     const y = Math.log((1 + sin) / (1 - sin)) / 2;
//     return clamp(y / Math.PI, -1, 1);
// }

// function getCenterFromBounds(bounds: [[number, number], [number, number]]) {
//     const [[minLng, minLat], [maxLng, maxLat]] = bounds;

//     return {
//         lng: (minLng + maxLng) / 2,
//         lat: (minLat + maxLat) / 2,
//     };
// }

// function getBoundsZoomEstimate(
//     bounds: [[number, number], [number, number]],
//     viewportWidth: number,
//     viewportHeight: number,
//     padding: number,
//     minZoom: number,
//     maxZoom: number
// ): number {
//     const WORLD_SIZE = 512;
//     const [[minLng, minLat], [maxLng, maxLat]] = bounds;

//     const usableWidth = Math.max(viewportWidth - padding * 2, 64);
//     const usableHeight = Math.max(viewportHeight - padding * 2, 64);

//     const lngDiff = Math.abs(maxLng - minLng);
//     const lngFraction = Math.max(lngDiff / 360, 1e-9);

//     const maxLatY = latToMercatorYFraction(maxLat);
//     const minLatY = latToMercatorYFraction(minLat);
//     const latFraction = Math.max(Math.abs(maxLatY - minLatY), 1e-9);

//     const zoomLng = Math.log2(usableWidth / (WORLD_SIZE * lngFraction));
//     const zoomLat = Math.log2(usableHeight / (WORLD_SIZE * latFraction));

//     const estimated = Math.min(zoomLng, zoomLat, maxZoom);
//     return clamp(estimated, minZoom, maxZoom);
// }

// export function getInitialViewStateForLocations(
//     locations: MapLocation[],
//     options: Pick<FocusOptions, "padding" | "maxZoom" | "minZoom" | "singlePointZoom"> & {
//         viewportWidth?: number;
//         viewportHeight?: number;
//     } = {}
// ): InitialViewState | null {
//     if (locations.length === 0) return null;

//     if (locations.length === 1) {
//         return {
//             longitude: locations[0].lng,
//             latitude: locations[0].lat,
//             zoom: options.singlePointZoom ?? 13,
//         };
//     }

//     const bounds = buildBounds(locations);
//     if (!bounds) return null;

//     const center = getCenterFromBounds(bounds);
//     const viewportWidth =
//         options.viewportWidth ?? (typeof window !== "undefined" ? window.innerWidth : 1280);
//     const viewportHeight =
//         options.viewportHeight ?? (typeof window !== "undefined" ? window.innerHeight : 720);

//     const zoom = getBoundsZoomEstimate(
//         bounds,
//         viewportWidth,
//         viewportHeight,
//         options.padding ?? 96,
//         options.minZoom ?? 1,
//         options.maxZoom ?? 14.5
//     );

//     return {
//         longitude: center.lng,
//         latitude: center.lat,
//         zoom,
//     };
// }

// export function getCenterAndZoomForLocations(
//     map: MapboxMap,
//     locations: MapLocation[],
//     options: Pick<FocusOptions, "padding" | "maxZoom" | "singlePointZoom"> = {}
// ): CenterAndZoom | null {
//     if (locations.length === 0) return null;

//     if (locations.length === 1) {
//         const only = locations[0];
//         return {
//             center: { lng: only.lng, lat: only.lat },
//             zoom: options.singlePointZoom ?? 13,
//         };
//     }

//     const bounds = buildBounds(locations);
//     if (!bounds) return null;

//     const camera = map.cameraForBounds(bounds, {
//         padding: options.padding ?? 96,
//         maxZoom: options.maxZoom ?? 14.5,
//     });

//     if (!camera.center || camera.zoom === undefined) return null;

//     return {
//         center: { lng: camera.center.lng, lat: camera.center.lat },
//         zoom: camera.zoom,
//     };
// }

// export function focusMapToLocations(
//     map: MapboxMap,
//     locations: MapLocation[],
//     options: FocusOptions = {}
// ) {
//     const target = getCenterAndZoomForLocations(map, locations, options);
//     if (!target) return;

//     const currentCenter = map.getCenter();
//     const currentZoom = map.getZoom();

//     const distanceKm = GetDistance(currentCenter.lat, currentCenter.lng, target.center.lat, target.center.lng);
//     const zoomDelta = Math.abs(currentZoom - target.zoom);

//     if (
//         distanceKm < (options.distanceThresholdKm ?? 0.35) &&
//         zoomDelta < (options.zoomThreshold ?? 0.25)
//     ) {
//         return;
//     }

//     map.easeTo({
//         center: [target.center.lng, target.center.lat],
//         zoom: target.zoom,
//         duration: options.durationMs ?? 1000,
//     });
// }
