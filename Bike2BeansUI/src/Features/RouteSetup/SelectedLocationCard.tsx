import type { LocationChoice } from "./locationSearch";

type Props = {
    label: string;
    location: LocationChoice;
    onChange: () => void;
    onRemove?: () => void;
};

export function SelectedLocationCard({ label, location, onChange, onRemove }: Props) {
    return (
        <section className="panel route-location-card route-location-static-card">
            <div className="route-location-card-head">
                <p className="route-location-card-label">{label}</p>
                <div className="route-location-card-actions">
                    <button type="button" className="btn-secondary" onClick={onChange}>
                        Change
                    </button>
                    {onRemove ? (
                        <button type="button" className="btn" onClick={onRemove}>
                            Remove stop
                        </button>
                    ) : null}
                </div>
            </div>
            <p className="route-location-card-title">{location.name}</p>
            <p className="muted">{location.address}</p>
        </section>
    );
}
