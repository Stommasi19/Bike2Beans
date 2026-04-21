import { GoogleAuthProvider, signInWithEmailAndPassword, signInWithPopup } from 'firebase/auth';
import { type FormEvent, type MouseEvent, useState } from 'react';
import { auth } from '../firebase';
import { getFirebaseErrorMessage, Toast } from "../Components/Toast.web";

function getErrorCode(error: unknown) {
    if (typeof error === "object" && error !== null && "code" in error) {
        return String(error.code);
    }

    return "unknown";
}


export function Login() {
    const [toast, setToast] = useState<string | null>(null);
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [loading, setLoading] = useState(false);
    const handleLogin = async (event: FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        setLoading(true);
        try {
            await signInWithEmailAndPassword(auth, email, password);
            window.location.href = "/home";


        } catch (error) {
            const message = getFirebaseErrorMessage(getErrorCode(error));
            setToast(message);
        } finally {
            setLoading(false);
        }
    }
    const handleGoogleSignIn = async (event: MouseEvent<HTMLButtonElement>) => {
        event.preventDefault();
        setLoading(true);
        try {
            await signInWithPopup(auth, new GoogleAuthProvider());
            window.location.href = "/home";

        } catch (error) {
            const message = getFirebaseErrorMessage(getErrorCode(error));
            setToast(message);
        } finally {
            setLoading(false);
        }
    }
    return (
        <div className="">{toast && (
            <Toast
                message={toast}
                onClose={() => setToast(null)}
            />
        )}
            <div className="login-page">

                <div className="login">
                    <h1>Welcome Back to Bike2Beans</h1>
                    <form onSubmit={handleLogin}>

                        <div className="signin">
                            <span className="loginText">Email Address</span>
                            <input
                                className='signInInput'
                                value={email}
                                onChange={(e) => setEmail(e.target.value)}
                                placeholder="Email"
                                type="email"
                            />
                        </div>
                        <div className="signin">
                            <span className="loginText">Password</span>
                            <input
                                className='signInInput'
                                value={password}
                                onChange={(e) => setPassword(e.target.value)}
                                placeholder="Password"
                                type="password"
                            />
                        </div>
                        <div className="loginbtn">
                            <button className="btn-primary" type='submit' disabled={loading}>
                                {loading ? "Signing In..." : "Sign In"}
                            </button>

                            <button className="btn" type="button" onClick={handleGoogleSignIn}>Sign In With Google</button>
                        </div> <br />

                        <span className='noAccount'>Don't have an account? <a className="link" href="/signup">Sign Up</a>
                        </span>
                    </form>
                </div>
            </div>
        </div>
    );
}
