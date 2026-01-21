import { Link } from "react-router-dom";

export default function NavBar() {


    return (
        <div id="navbar">
            <nav>
                <ul>
                    <li>
                        <Link rel="" to="/Login"> Login </Link>
                    </li>
                    <li>
                        <Link rel="" to="/Map"> Map </Link>
                    </li>
                    <li>
                        <Link rel="" to="/CoffeeShops"> Coffee Shops </Link>
                    </li>
                </ul>
            </nav>
        </div>
    )
}