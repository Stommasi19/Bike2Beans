import React from "react";

export function Login() {
    return (
        <div className="login-page">
            <div className="login">
                <h1>Welcome to Bike2Beans</h1>

                <div className="signin">
                    <span className="loginText">Username</span> <br />
                    <input className="input" type="text" name="username" id="username" placeholder="username" />
                </div>
                <div className="signin">
                    <span className="loginText">Password</span> <br />
                    <input className="input" type="password" name="password" id="password" placeholder="password" />
                </div>
                <div className="loginbtn">
                    <a className="btn-primary" href="/home">Sign in</a>
                    <a className="btn-secondary" href="/signup">Sign Up</a>
                </div>
                <div className="loginbtn">
                    <button className="btn" type="button">Sign In With Google</button>
                </div>
            </div>
        </div>
    );
}

