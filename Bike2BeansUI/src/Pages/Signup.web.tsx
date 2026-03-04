import React from "react";

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
                    <input className="input" type="email" name="email" id="email" placeholder="Email" />
                </div>
                <div className="signin">
                    <span className="loginText">Username</span> <br />
                    <input className="input" type="text" name="username" id="username" placeholder="username" />
                </div>
                <div className="signin">
                    <span className="loginText">Password</span> <br />
                    <input className="input" type="password" name="password" id="password" placeholder="password" />
                </div>
                <div className="loginbtn">
                    <a className="btn-primary" href="/home">Sign Up</a>
                    <a className="btn-secondary" href="/login">Sign In</a>
                </div>
                <div className="loginbtn">
                    <button className="btn" type="button">Sign Up With Google</button>
                </div>
            </div>
        </div>
    );
}

