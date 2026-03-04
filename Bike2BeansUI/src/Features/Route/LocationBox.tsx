import { CoffeeShopDto } from "../../Data/coffeeshopsDto"
type Props = {
    routeStops: RouteDto[];
    reorderStops: (shops: RouteDto[]) => void;
    removeStop: (stopId: string) => void
}
import {
    DndContext,
    closestCenter,
    PointerSensor,
    KeyboardSensor,
    useSensor,
    useSensors,
    DragEndEvent,
} from "@dnd-kit/core";
import {
    SortableContext,
    verticalListSortingStrategy,
    arrayMove,
    sortableKeyboardCoordinates,
} from "@dnd-kit/sortable";
import { SortableShopRow } from "./SortableShopRow";
import { RouteDto } from "../../Data/RouteDto";

export function LocationBox({ routeStops, reorderStops, removeStop }: Props) {
    const sensors = useSensors(
        useSensor(PointerSensor, {
            activationConstraint: { distance: 6 },
        }),
        useSensor(KeyboardSensor, {
            coordinateGetter: sortableKeyboardCoordinates,
        })
    );

    function handleDragEnd(event: DragEndEvent) {
        const { active, over } = event;
        if (!over) return;

        if (active.id === over.id) return;

        const oldIndex = routeStops.findIndex((rs) => rs.stopId === active.id)
        const newIndex = routeStops.findIndex((rs) => rs.stopId === over.id)

        reorderStops(arrayMove(routeStops, oldIndex, newIndex))
    }

    return (
        <div className="location-box">
            <DndContext
                sensors={sensors}
                collisionDetection={closestCenter}
                onDragEnd={handleDragEnd}
            >
                <SortableContext
                    items={routeStops.map((s) => s.stopId)}
                    strategy={verticalListSortingStrategy}>
                    {routeStops.map((routeStop) => (
                        <SortableShopRow key={routeStop.stopId} routeStop={routeStop} reorderStops={reorderStops} removeStop={removeStop} />
                    ))}

                </SortableContext>

            </DndContext>
        </div>
    );
}