import { SearchResultCard } from "./SearchCard"
import { SearchState } from "../../Data/SearchState"
import { useEffect, useId, useState } from "react"
import { getAutocomplete, type AutocompleteSuggestion } from "../../Api/Autocomplete"

type Props = {
    getCoffeeshopFromAutocomplete: (text: string | null) => void;
}

export function Search({ getCoffeeshopFromAutocomplete }: Props) {
    var [searchState, setSearchState] = useState<SearchState>(SearchState.Idle)
    const [autocompleteResults, setAutocompleteResults] = useState<AutocompleteSuggestion[]>([]);
    const [query, setQuery] = useState("");
    const [error, setError] = useState<string | null>(null);
    const resultsId = useId();

    useEffect(() => {
        const normalizedQuery = query.trim();
        if (normalizedQuery.length <= 2) {
            setAutocompleteResults([]);
            setError(null);
            setSearchState(SearchState.Idle);
            return undefined;
        }

        let canceled = false;
        const timer = setTimeout(async () => {
            try {
                setSearchState(SearchState.Loading);
                setError(null);
                const results = await getAutocomplete(normalizedQuery);
                if (canceled) return;

                setAutocompleteResults(results)
                setSearchState(SearchState.Active)
            } catch (error) {
                if (canceled) return;

                setError("Search suggestions are unavailable.");
                setAutocompleteResults([]);
                setSearchState(SearchState.Idle);
            }
        }, 500);

        return () => {
            canceled = true;
            clearTimeout(timer);
        };
    }, [query]);

    const handleSearch = (e: React.ChangeEvent<HTMLInputElement>) => {
        setQuery(e.target.value);
    };

    const clearSearch = () => {
        setQuery("");
        setAutocompleteResults([]);
        setError(null);
        setSearchState(SearchState.Idle);
    };

    return (
        <div className="searchbox" data-state={searchState}>
            <form action=""
                className="searchbar"
                onSubmit={(e) => e.preventDefault()}>
                <input type="text"
                    placeholder="Search..."
                    value={query}
                    onChange={handleSearch}
                    className="searchbar-input"
                    autoComplete="off"
                    aria-controls={resultsId}
                    aria-expanded={searchState === SearchState.Active || searchState === SearchState.Loading}
                />
                <button
                    type="button"
                    onClick={clearSearch}
                    disabled={!query && autocompleteResults.length === 0}
                    className="clear-search-button" >Clear</button>
            </form>

            <div className="searchbox-results-card" id={resultsId} role="listbox" aria-label="Coffee shop search suggestions">
                {error ? <p className="searchbox-message" role="alert">{error}</p> : null}
                {autocompleteResults.length > 0 ? (
                    autocompleteResults.map((result) => (
                        <button
                            type="button"
                            key={result.text ?? result.Text ?? "suggestion"}
                            className="searchbox-result-button"
                            onClick={() => getCoffeeshopFromAutocomplete(result.text ?? result.Text ?? null)}
                        >
                            <SearchResultCard
                                text={result.text ?? result.Text ?? ""} />
                        </button>
                    ))
                ) : searchState === SearchState.Loading ? (
                    <SearchResultCard text={"... Searching"} />
                ) : searchState === SearchState.Active ? (
                    <p className="searchbox-message">No suggestions found.</p>
                ) : null}
            </div>

        </div>
    )
}
