import React from "react";

const navItems = [
    { href: "/login", label: "Login" },
    { href: "/home", label: "Home" },
    { href: "/saved-routes", label: "Saved Routes" },
];

export function NavBar() {
    return (
        <div id="navbar" className="navbar">
            <nav>
                <ul>
                    {navItems.map((item) => (
                        <li key={item.href}>
                            <a href={item.href}>{item.label}</a>
                        </li>
                    ))}
                </ul>
            </nav>
        </div>
    );
}

