import { SearchResultCard } from "./SearchCard"
import { SearchState } from "../../Data/SearchState"
import { useEffect, useState } from "react"
import { getAutocomplete } from "../../Api/Autocomplete"
import { searchPlacesByText } from "../../Api/Places"
import { CoffeeshopDto } from "../../Data/CoffeeshopDto"

type Props = {
    getCoffeeshopFromAutocomplete: (text: string | null) => void;
}

export function Search({ getCoffeeshopFromAutocomplete }: Props) {
    var [searchState, setSearchState] = useState<SearchState>(SearchState.Idle)
    const [autocompleteResults, setAutocompleteResults] = useState<{ text: string | null }[]>([]);

    const [query, setQuery] = useState("");

    const handleSearch = async (e: React.ChangeEvent<HTMLInputElement>) => {
        const value = e.target.value;
        setQuery(value);

        if (value.trim().length <= 2) {
            setAutocompleteResults([]);
            setSearchState(SearchState.Idle);
            return;
        }

        try {
            setTimeout(async () => {

                setSearchState(SearchState.Loading);
                const results = await getAutocomplete(value);
                console.log(results)

                setAutocompleteResults(results)
                setSearchState(SearchState.Active)
            }, 500);
        } catch (error) {
            console.error("Autocomplete failed:", error);
            setAutocompleteResults([]);
            setSearchState(SearchState.Idle);
        }
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
                <button onClick={() => setSearchState(SearchState.Idle)}
                    className="clear-search-button" >Clear</button>
            </form>

            <div className="searchbox-results-card">

                {autocompleteResults.length > 0 ?
                    (autocompleteResults.map((result, i) => (
                        <button
                            key={i}
                            onClick={() => getCoffeeshopFromAutocomplete(result.text)}
                        >
                            <SearchResultCard
                                key={i}
                                text={result.text} />
                        </button>
                    ))
                    ) : (
                        <SearchResultCard text={"... Searching"} />
                    )}
            </div>

        </div>
    )
}