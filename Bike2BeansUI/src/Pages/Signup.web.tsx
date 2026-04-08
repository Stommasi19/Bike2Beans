import React, { useState } from "react";
import { createUserWithEmailAndPassword, GoogleAuthProvider, signInWithPopup } from 'firebase/auth';
import { auth } from '../Firebase';
import { Toast, getFirebaseErrorMessage } from "../Components/Toast.web";
export function Signup() {
    const [toast, setToast] = useState<string | null>(null);
    const [first, setFirst] = useState("");
    const [last, setLast] = useState("");
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState("");
    const [loading, setLoading] = useState(false);
    const handleSignUp = async (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        setError("");
        setLoading(true);
        try {
            await createUserWithEmailAndPassword(auth, email, password);
            window.location.href = "/home";

        } catch (err: any) {
            const message = getFirebaseErrorMessage(err.code);
            setToast(message);
        } finally {
            setLoading(false);
        }
    }
    const handleGoogleSignUp = async (event: React.MouseEvent<HTMLButtonElement>) => {
        event.preventDefault();
        setLoading(true);
        try {
            await signInWithPopup(auth, new GoogleAuthProvider());
            window.location.href = "/home";

        }
        catch (err: any) {
            const message = getFirebaseErrorMessage(err.code);
            setToast(message);
        } finally {
            setLoading(false);
        }


    }

    return (
        <div className="login-page">
            {toast && (
                <Toast
                    message={toast}
                    onClose={() => setToast(null)}
                />
            )}
            <div className="signup">
                <h1>Welcome to Bike2Beans</h1>

                <form onSubmit={handleSignUp}>
                    <div className="signin">
                        <span className="loginText">First Name</span>
                        <input className="input"
                            type="text"
                            name="first name"
                            id="firstname"
                            placeholder="First Name"
                            onChange={(e) => setFirst(e.target.value)}
                        />
                    </div><div className="signin">
                        <span className="loginText">Last Name</span>
                        <input className="input"
                            type="text"
                            name="lastname"
                            id="lastname"
                            placeholder="Last Name"
                            onChange={(e) => setLast(e.target.value)}
                        />
                    </div>

                    <div className="signin">
                        <span className="loginText">Email</span>
                        <input className="input"
                            type="email"
                            name="email"
                            id="email"
                            placeholder="Email"
                            onChange={(e) => setEmail(e.target.value)}
                        />
                    </div>
                    <div className="signin">
                        <span className="loginText">Password</span>
                        <input className="input"
                            type="password" name="password"
                            id="password"
                            placeholder="password"
                            onChange={(e) => setPassword(e.target.value)}
                        />
                    </div>
                    <div className="loginbtn">
                        <button className="btn-primary" type="submit">
                            {loading ? "Signing Up..." : "Sign Up"}
                        </button>

                        <button className="btn" type="button" onClick={handleGoogleSignUp}>
                            Sign Up With Google
                        </button>
                    </div>
                    <span className='noAccount'>Have an account? <a className="link" href="/login">Sign In</a>
                    </span>

                </form>
            </div>
        </div>
    );
}



