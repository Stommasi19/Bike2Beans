import { Link } from "react-router-dom"

export function Signup() {


    return (
        <div className="login-page">
            <div className="signup">

                <h1>Welcome to Bike2Beans</h1>


                <div className="signin">
                    <span className="loginText">Name</span>
                    <input className="input" type="text" name="name" id="name" placeholder="Name" />
                </div>
                <div className="signin">
                    <span className="loginText">Email</span>
                    <input className="input" type="email" name="" id="" placeholder="Email" />
                </div>
                <div className="signin">
                    <span className="loginText">Username</span> <br />
                    <input className="input" type="text" name="username" id="username" placeholder="username" />
                </div>
                <div className="signin">
                    <span className="loginText">Password</span> <br />
                    <input className="input" type="password" name="" id="password" placeholder="password" />
                </div>
                <div className="loginbtn">
                    <Link to={"/Home"}>
                        <button className="btn-primary">Sign Up</button>
                    </Link>
                    <Link to="/Login">
                        <button className="btn-secondary">Sign In</button>
                    </Link>
                </div>
                <br />
                <div className="loginbtn">
                    <button className="btn"> Sign Up With Google</button>
                </div>

            </div>
        </div>
    )
}
