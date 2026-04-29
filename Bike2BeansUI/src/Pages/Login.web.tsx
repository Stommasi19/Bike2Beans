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
    const canSubmit = email.trim().length > 0 && password.length > 0 && !loading;

    const handleLogin = async (event: FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        if (!canSubmit) return;

        setLoading(true);
        try {
            await signInWithEmailAndPassword(auth, email.trim(), password);
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
        if (loading) return;

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
        <div className="auth-page">{toast && (
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
                            <label className="loginText" htmlFor="login-email">Email Address</label>
                            <input
                                id="login-email"
                                className='signInInput'
                                value={email}
                                onChange={(e) => setEmail(e.target.value)}
                                placeholder="Email"
                                type="email"
                                autoComplete="email"
                                inputMode="email"
                                required
                                disabled={loading}
                            />
                        </div>
                        <div className="signin">
                            <label className="loginText" htmlFor="login-password">Password</label>
                            <input
                                id="login-password"
                                className='signInInput'
                                value={password}
                                onChange={(e) => setPassword(e.target.value)}
                                placeholder="Password"
                                type="password"
                                autoComplete="current-password"
                                required
                                disabled={loading}
                            />
                        </div>
                        <div className="loginbtn">
                            <button className="btn-primary" type='submit' disabled={!canSubmit}>
                                {loading ? "Signing In..." : "Sign In"}
                            </button>

                            <button className="btn" type="button" onClick={handleGoogleSignIn} disabled={loading}>Sign In With Google</button>
                        </div> <br />

                        <span className='noAccount'>Don't have an account? <a className="link" href="/signup">Sign Up</a>
                        </span>
                    </form>
                </div>
            </div>
        </div>
    );
}
