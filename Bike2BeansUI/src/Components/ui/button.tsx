import * as React from "react";
import { Slot } from "@radix-ui/react-slot";
import { cva, type VariantProps } from "class-variance-authority";

import { cn } from "@/lib/utils";

const buttonVariants = cva(
    "inline-flex items-center justify-center gap-2 whitespace-nowrap rounded-md text-sm font-medium transition-all disabled:pointer-events-none disabled:opacity-50 [&_svg]:pointer-events-none [&_svg:not([class*='size-'])]:size-4 shrink-0 [&_svg]:shrink-0 outline-none focus-visible:ring-2 focus-visible:ring-blue-300",
    {
        variants: {
            variant: {
                default: "bg-slate-900 text-white shadow-sm hover:bg-slate-700",
                destructive: "bg-red-600 text-white shadow-sm hover:bg-red-500",
                outline: "border border-slate-300 bg-white text-slate-900 shadow-sm hover:bg-slate-50",
                secondary: "bg-slate-200 text-slate-900 shadow-sm hover:bg-slate-300",
                ghost: "text-slate-800 hover:bg-slate-100",
                link: "text-blue-700 underline-offset-4 hover:underline",
            },
            size: {
                default: "h-9 px-4 py-2",
                sm: "h-8 rounded-md gap-1.5 px-3",
                lg: "h-10 rounded-md px-6",
                icon: "size-9",
            },
        },
        defaultVariants: {
            variant: "default",
            size: "default",
        },
    }
);

function Button({
    className,
    variant,
    size,
    asChild = false,
    ...props
}: React.ComponentProps<"button"> &
    VariantProps<typeof buttonVariants> & {
        asChild?: boolean;
    }) {
    const Comp = asChild ? Slot : "button";

    return (
        <Comp
            data-slot="button"
            className={cn(buttonVariants({ variant, size, className }))}
            {...props}
        />
    );
}

export { Button, buttonVariants };
