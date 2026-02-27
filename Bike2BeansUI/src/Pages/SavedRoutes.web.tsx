import React, { useEffect } from "react";
import { GetRoutes } from "../Api/RouteDetails";

export function SavedRoutes() {

    const [routes, setRoutes] = React.useState([]);

    useEffect(() => {
        GetRoutes()
            .then(setRoutes)
            .catch(console.error);

    }, [])

    console.log("routes", routes)
    return (
        <div>
            <section className="panel center">
                <h2 className="section-title">Saved Routes</h2>
                {routes.length === 0 ? <p className="muted">Saved route previews will appear here.</p> : null}
            </section>
            <div>
                <br />
                {routes.map((stop) => (
                    <div className="saved_route_card" key={stop.id}>
                        <img src="/dino.png" alt="" />
                        <div className="center">
                            <h1>{stop.name}</h1>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
}

