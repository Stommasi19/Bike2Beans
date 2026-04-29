import { useEffect, useState } from "react";
import { GetRoutes, type SavedRoutePreview } from "../Api/RouteDetails";

export function SavedRoutes() {
    const [routes, setRoutes] = useState<SavedRoutePreview[]>([]);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

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

    return (
        <div className="page-shell saved-routes-page">
            <section className="panel center">
                <h2 className="section-title">Saved Routes</h2>
                {isLoading ? <p className="muted" role="status">Loading saved routes...</p> : null}
                {error ? <p className="form-error" role="alert">{error}</p> : null}
                {!isLoading && !error && routes.length === 0 ? <p className="muted">Saved route previews will appear here.</p> : null}
            </section>
            <div className="saved-routes-grid">
                {routes.map((stop) => (
                    <div className="saved_route_card" key={stop.id}>
                        <img src="/dino.png" alt="" loading="lazy" />
                        <div className="center">
                            <h1>{stop.name}</h1>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
}
