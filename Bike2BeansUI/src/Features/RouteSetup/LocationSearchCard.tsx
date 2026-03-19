type Props = {
    label: string;
    query: string;
    suggestions: string[];
    onQueryChange: (value: string) => void;
    onSelectSuggestion: (suggestion: string) => void;
    onCancel?: () => void;
};

export function LocationSearchCard({
    label,
    query,
    suggestions,
    onQueryChange,
    onSelectSuggestion,
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
                placeholder="Search for an address..."
            />
            {hasQuery ? (
                <div className="route-location-results" role="listbox" aria-label={`${label} autocomplete results`}>
                    {suggestions.length === 0 ? (
                        <p className="route-location-empty muted">No suggestions found.</p>
                    ) : (
                        suggestions.map((suggestion) => (
                            <button
                                type="button"
                                key={suggestion}
                                className="route-location-result"
                                onClick={() => onSelectSuggestion(suggestion)}
                            >
                                <span className="route-location-result-title">{suggestion}</span>
                            </button>
                        ))
                    )}
                </div>
            ) : null}
        </section>
    );
}
