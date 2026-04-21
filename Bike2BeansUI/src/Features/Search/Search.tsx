import { SearchResultCard } from "./SearchCard"
import { SearchState } from "../../Data/SearchState"
import { useEffect, useState } from "react"
import { getAutocomplete, type AutocompleteSuggestion } from "../../Api/Autocomplete"

type Props = {
    getCoffeeshopFromAutocomplete: (text: string | null) => void;
}

export function Search({ getCoffeeshopFromAutocomplete }: Props) {
    var [searchState, setSearchState] = useState<SearchState>(SearchState.Idle)
    const [autocompleteResults, setAutocompleteResults] = useState<AutocompleteSuggestion[]>([]);
    const [query, setQuery] = useState("");

    useEffect(() => {
        const normalizedQuery = query.trim();
        if (normalizedQuery.length <= 2) {
            setAutocompleteResults([]);
            setSearchState(SearchState.Idle);
            return undefined;
        }

        let canceled = false;
        const timer = setTimeout(async () => {
            try {
                setSearchState(SearchState.Loading);
                const results = await getAutocomplete(normalizedQuery);
                if (canceled) return;

                setAutocompleteResults(results)
                setSearchState(SearchState.Active)
            } catch (error) {
                if (canceled) return;

                console.error("Autocomplete failed:", error);
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
                />
                <button
                    type="button"
                    onClick={() => setSearchState(SearchState.Idle)}
                    className="clear-search-button" >Clear</button>
            </form>

            <div className="searchbox-results-card">
                {autocompleteResults.length > 0 ? (
                    autocompleteResults.map((result) => (
                        <button
                            type="button"
                            key={result.text ?? result.Text ?? "suggestion"}
                            onClick={() => getCoffeeshopFromAutocomplete(result.text ?? result.Text ?? null)}
                        >
                            <SearchResultCard
                                text={result.text ?? result.Text ?? ""} />
                        </button>
                    ))
                ) : searchState === SearchState.Loading ? (
                    <SearchResultCard text={"... Searching"} />
                ) : null}
            </div>

        </div>
    )
}
