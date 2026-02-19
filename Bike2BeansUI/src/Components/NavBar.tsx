import { Link } from "react-router-dom";

export function NavBar() {


    return (
        <div id="navbar" className="navbar">
            <nav>
                <ul>
                    <li>
                        <Link rel="" to="/Login"> Login </Link>
                    </li>
                    <li>
                        <Link rel="" to="/Home"> Home </Link>
                    </li>

                    <li>
                        <Link rel="" to="/SavedRoutes"> Saved Routes</Link>
                    </li>

                </ul>
            </nav>
        </div>
    )
}
