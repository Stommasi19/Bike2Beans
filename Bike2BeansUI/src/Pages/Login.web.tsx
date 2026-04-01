import { GoogleAuthProvider, signInWithCredential, signInWithEmailAndPassword, signInWithPopup } from 'firebase/auth';
import { useState } from 'react';
import { auth } from '../Firebase';
import { getFirebaseErrorMessage, Toast } from "../Components/Toast.web";


export function Login() {
    const [toast, setToast] = useState<string | null>(null);
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState("");
    const [loading, setLoading] = useState(false);
    const handleLogin = async (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        setError("");
        setLoading(true);
        try {
            await signInWithEmailAndPassword(auth, email, password);

        } catch (err: any) {
            const message = getFirebaseErrorMessage(err.code);
            setToast(message);
        } finally {
            setLoading(false);
            // navigate to app
            window.location.href = "/home";
        }
    }
    const handleGoogleSignIn = async (event: React.MouseEvent<HTMLButtonElement>) => {
        event.preventDefault();
        try {
            await signInWithPopup(auth, new GoogleAuthProvider());
        } catch (err: any) {
            const message = getFirebaseErrorMessage(err.code);
            setToast(message);
        } finally {
            // navigate to app
            window.location.href = "/home";
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

