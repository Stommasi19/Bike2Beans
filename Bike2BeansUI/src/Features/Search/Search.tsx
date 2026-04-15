import { SearchResultCard } from "./SearchCard"
import { SearchState } from "../../Data/SearchState"
import { useEffect, useState } from "react"

export function Search() {
    var [searchState, setSearchState] = useState<SearchState>(SearchState.Loading)
    const [autocompleteresults, setAutocompleteresults] = useState<string[]>([])

    const [query, setQuery] = useState("");

    return (
        <div className="searchbox" data-state={searchState}>
            <form action=""
                className="searchbar"
                onSubmit={(e) => e.preventDefault()}>
                <input type="text"
                    placeholder="Search..."
                    value={query}
                    // onChange={handleSearch}
                    className="searchbar-input"
                />
                <button onClick={() => setSearchState(SearchState.Idle)}
                    className="clear-search-button" >Clear</button>
            </form>

            <div className="searchbox-results-card">
                {autocompleteresults.length > 0 ?
                    (autocompleteresults.map((result, i) => (
                        <SearchResultCard key={i} text={result} />
                    ))
                    ) : (
                        <SearchResultCard text={"...searching"} />
                    )}
            </div>

        </div>
    )
}