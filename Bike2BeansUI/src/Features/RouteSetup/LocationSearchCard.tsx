import type { LocationChoice } from "./locationSearch";

type Props = {
    label: string;
    query: string;
    results: LocationChoice[];
    onQueryChange: (value: string) => void;
    onSelect: (location: LocationChoice) => void;
    onCancel?: () => void;
    onEnter?: () => void;
};

export function LocationSearchCard({
    label,
    query,
    results,
    onQueryChange,
    onSelect,
    onCancel,
}: Props) {
    const hasQuery = query.trim().length > 0;

    return (
        <section className="panel route-location-card">
            <div className="route-location-card-head">
                <p className="route-location-card-label">{label}</p>
                {onCancel ? (
                    <button type="button" className="btn route-inline-cancel" onClick={onCancel}>
                        Cancel
                    </button>
                ) : null}
            </div>
            <input
                className="input route-location-input"
                type="text"
                value={query}
                onChange={(event) => onQueryChange(event.target.value)}
                placeholder="Search for a location..."
            />
            {hasQuery ? (
                <div className="route-location-results" role="listbox" aria-label={`${label} search results`}>
                    {results.length === 0 ? (
                        <p className="route-location-empty muted">No matching locations found.</p>
                    ) : (
                        results.map((location) => (
                            <button
                                type="button"
                                key={location.id}
                                className="route-location-result"
                                onClick={() => onSelect(location)}
                            >
                                <span className="route-location-result-title">{location.name}</span>
                                <span className="route-location-result-address">{location.address}</span>
                            </button>
                        ))
                    )}
                </div>
            ) : null}
        </section>
    );
}
