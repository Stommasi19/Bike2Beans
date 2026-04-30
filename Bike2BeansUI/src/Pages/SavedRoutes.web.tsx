import { useNavigation } from "@react-navigation/native";
import type { NativeStackNavigationProp } from "@react-navigation/native-stack";
import { useEffect, useState } from "react";
import { GetRoutes, type SavedRoutePreview } from "../Api/RouteDetails";
import type { RootStackParamList } from "../Navigation/types";

function RoutePreviewMap({ routeName, isActive }: { routeName: string; isActive: boolean }) {
    return (
        <div className="saved-route-map-preview" aria-hidden="true" data-active={isActive}>
            <div className="saved-route-map-grid" />
            <div className="saved-route-map-road saved-route-map-road-main" />
            <div className="saved-route-map-road saved-route-map-road-side" />
            <svg className="saved-route-map-line" viewBox="0 0 320 180" role="img" aria-label={`${routeName} map preview`}>
                <path d="M26 136 C72 84, 104 152, 146 98 S226 44, 292 82" />
            </svg>
            <span className="saved-route-map-pin saved-route-map-pin-start" />
            <span className="saved-route-map-pin saved-route-map-pin-end" />
        </div>
    );
}

export function SavedRoutes() {
    const navigation = useNavigation<NativeStackNavigationProp<RootStackParamList>>();
    const [routes, setRoutes] = useState<SavedRoutePreview[]>([]);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [expandedRouteId, setExpandedRouteId] = useState<string | null>(null);

    useEffect(() => {
        let isMounted = true;

        GetRoutes()
            .then((nextRoutes) => {
                if (!isMounted) return;
                setRoutes(nextRoutes);
                setError(null);
            })
            .catch(() => {
                if (!isMounted) return;
                setRoutes([]);
                setError("Saved routes could not be loaded. Please try again in a moment.");
            })
            .finally(() => {
                if (isMounted) setIsLoading(false);
            });

        return () => {
            isMounted = false;
        };
    }, []);

    function editRoute(routePreview: SavedRoutePreview) {
        navigation.navigate("Home", {
            routeStops: [],
            routeEditNotice: `${routePreview.name} is available as a saved preview. Save route stop details to edit it on the map.`,
        });
    }

    return (
        <div className="page-shell saved-routes-page">
            <section className="saved-routes-header">
                <div>
                    <p className="route-options-label">Route library</p>
                    <h2 className="section-title">Saved Routes</h2>
                </div>
                {isLoading ? <p className="muted" role="status">Loading saved routes...</p> : null}
                {error ? <p className="form-error" role="alert">{error}</p> : null}
                {!isLoading && !error && routes.length === 0 ? (
                    <p className="muted">Save a route and it will appear here as a map card.</p>
                ) : null}
            </section>
            <div className="saved-routes-grid">
                {routes.map((stop) => (
                    <div className="saved_route_card" key={stop.id} data-active={expandedRouteId === stop.id}>
                        <button
                            type="button"
                            className="saved-route-map-button"
                            aria-expanded={expandedRouteId === stop.id}
                            aria-label={`Open saved route ${stop.name}`}
                            onClick={() => setExpandedRouteId((current) => current === stop.id ? null : stop.id)}
                        >
                            <RoutePreviewMap routeName={stop.name} isActive={expandedRouteId === stop.id} />
                        </button>
                        {expandedRouteId === stop.id ? (
                            <div className="saved-route-details">
                                <div>
                                    <p className="route-options-label">Saved preview</p>
                                    <h3>{stop.name}</h3>
                                </div>
                                <p className="muted">
                                    Route stop details are not included in this preview yet. You can open the map flow now, and saved editable stops can plug into this path later.
                                </p>
                                <button type="button" className="btn-primary" onClick={() => editRoute(stop)}>
                                    Edit in map
                                </button>
                            </div>
                        ) : null}
                    </div>
                ))}
            </div>
        </div>
    );
}
