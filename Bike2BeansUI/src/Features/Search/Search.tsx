import { SearchCard } from "./SearchCard"
import { SearchState } from "../../Data/SearchState"
import { useState } from "react"

export function Search() {
    var [searchState, setSearchState] = useState<SearchState>(SearchState.Active)
    const autocompleteresults: String[] = ["hello", "helllloo"]
    return (
        <div className="searchbox" data-state={searchState}>
            <form action=""
                className="searchbar"
                onSubmit={(e) => e.preventDefault()}>
                <input type="text"
                    placeholder="Search..."
                    // value={query}
                    // onChange={handleSearch}
                    className="searchbar-input"
                />
            </form>

            <div className="searchbox-results-card">
                {autocompleteresults.length > 0 ?
                    (autocompleteresults.map((result, i) => (
                        <SearchCard key={i} text={result} />
                    ))
                    ) : (
                        <SearchCard text={"...searching"} />
                    )}
            </div>

        </div>
    )
}