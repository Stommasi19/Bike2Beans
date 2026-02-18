import { CoffeeShopDto } from "../../Data/coffeeshopsDto";
import { useSortable } from "@dnd-kit/sortable";
import { CSS } from "@dnd-kit/utilities";
import { CoffeeShopCard } from "../CoffeeShop/CoffeeShopCards";
import { RouteDto } from "../../Data/RouteDto";
type Props = {
    routeStop: RouteDto;
    removeStop: (stopId: string) => void;
    reorderStops: (shops: RouteDto[]) => void;
}

export function SortableShopRow({ routeStop, removeStop, reorderStops }: Props) {
    const active = "route"
    const {
        attributes,
        listeners,
        setNodeRef,
        transform,
        transition,
        isDragging,
    } = useSortable({ id: routeStop.stopId });

    const style: React.CSSProperties = {
        transform: CSS.Transform.toString(transform),
        transition,
        opacity: isDragging ? 0.6 : 1,
        cursor: "grab",
    };

    return (
        <div ref={setNodeRef} style={style} className="location-row">

            <div className="drag-handle" {...attributes} {...listeners}>
                ⠿
            </div>
            <CoffeeShopCard shop={routeStop.shop} stopId={routeStop.stopId} active={active} removeStop={removeStop}
            />
        </div>
    )
}