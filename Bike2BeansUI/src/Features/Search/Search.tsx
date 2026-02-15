import { SearchCard } from "./SearchCard"
import { SearchState } from "../../Data/SearchState"
import { useState } from "react"

export function Search() {
    var [state, setState] = useState<SearchState>(SearchState.idle)
    const autocompleteresults: String[] = ["hello", "helllloo"]
    return (
        <div className="relative w-fit">
            <form action=""
                className="searchbar"
                onSubmit={(e) => e.preventDefault()}>
                <input type="text"
                    placeholder="Search..."
                    // value={query}
                    // onChange={handleSearch}
                    className="searchbar-input w-80"
                />
            </form>

            <div className="searchbar-results-card">
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