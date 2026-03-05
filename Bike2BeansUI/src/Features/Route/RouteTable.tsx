import { LocationBox } from "./LocationBox"
import { CoffeeshopDto } from "../../Data/CoffeeshopDto"
import { RouteDto } from "../../Data/RouteDto"
import { useNavigation } from "@react-navigation/native";
import { RootStackParamList } from "../../Navigation/types";
import { NativeStackNavigationProp } from "@react-navigation/native-stack";
import { useState } from "react";

type Props = {
    routeStops: RouteDto[];
    reorderStops: (shops: RouteDto[]) => void;
    removeStop: (stopId: string) => void;


}

export function RouteTable({ routeStops, reorderStops, removeStop }: Props) {
    const [query, setQuery] = useState();



    return (
        <div className="route-container center">
            <h1 className="element-header">
                Add Shops To A Route            </h1>

            <input
                className="input route-location-input"
                type="text"
                value={query}
                placeholder="Search for an address..."
            />
            {/* {hasQuery ? (
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
            ) : null} */}
            <LocationBox routeStops={routeStops} reorderStops={reorderStops} removeStop={removeStop} />

        </div>
    )
}