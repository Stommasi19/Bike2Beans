import type { RouteGeoJson } from "../Map/routeGeoJson";

function escapeXml(value: string) {
    return value
        .replaceAll("&", "&amp;")
        .replaceAll("<", "&lt;")
        .replaceAll(">", "&gt;")
        .replaceAll('"', "&quot;")
        .replaceAll("'", "&apos;");
}

export function convertGeoJSONToGPX(route: RouteGeoJson, name = "Bike2Beans route") {
    const trackPoints = route.geometry.coordinates
        .filter((coordinate) => (
            Array.isArray(coordinate)
            && coordinate.length >= 2
            && Number.isFinite(Number(coordinate[0]))
            && Number.isFinite(Number(coordinate[1]))
        ))
        .map(([longitude, latitude]) => (
            `      <trkpt lat="${Number(latitude)}" lon="${Number(longitude)}"></trkpt>`
        ));

    if (trackPoints.length < 2) {
        throw new Error("A GPX route requires at least two valid coordinates.");
    }

    const safeName = escapeXml(name);

    return [
        '<?xml version="1.0" encoding="UTF-8"?>',
        '<gpx version="1.1" creator="Bike2Beans" xmlns="http://www.topografix.com/GPX/1/1">',
        "  <metadata>",
        `    <name>${safeName}</name>`,
        "  </metadata>",
        "  <trk>",
        `    <name>${safeName}</name>`,
        "    <trkseg>",
        ...trackPoints,
        "    </trkseg>",
        "  </trk>",
        "</gpx>",
        "",
    ].join("\n");
}
