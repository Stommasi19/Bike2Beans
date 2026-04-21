import { type FormEvent, type MouseEvent, useState } from "react";
import { createUserWithEmailAndPassword, GoogleAuthProvider, signInWithPopup } from 'firebase/auth';
import { auth } from '../firebase';
import { Toast, getFirebaseErrorMessage } from "../Components/Toast.web";
import { CreateUser } from "../Api/User";

function getErrorCode(error: unknown) {
    if (typeof error === "object" && error !== null && "code" in error) {
        return String(error.code);
    }

    return "unknown";
}

function splitDisplayName(displayName: string | null, email: string | null) {
    const fallbackName = email?.split("@")[0]?.trim() ?? "";
    const normalizedName = displayName?.trim() || fallbackName;
    const [firstName = "", ...rest] = normalizedName.split(/\s+/).filter(Boolean);

    return {
        firstName,
        lastName: rest.join(" "),
    };
}

export function Signup() {
    const [toast, setToast] = useState<string | null>(null);
    const [first, setFirst] = useState("");
    const [last, setLast] = useState("");
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [loading, setLoading] = useState(false);
    const handleSignUp = async (event: FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        setLoading(true);
        try {
            await createUserWithEmailAndPassword(auth, email, password);
            await CreateUser(first, last);
            window.location.href = "/home";

        } catch (error) {
            const message = getFirebaseErrorMessage(getErrorCode(error));
            setToast(message);
        } finally {
            setLoading(false);
        }
    }
    const handleGoogleSignUp = async (event: MouseEvent<HTMLButtonElement>) => {
        event.preventDefault();
        setLoading(true);
        try {
            const result = await signInWithPopup(auth, new GoogleAuthProvider());
            const { firstName, lastName } = splitDisplayName(result.user.displayName, result.user.email);
            await CreateUser(firstName, lastName);
            window.location.href = "/home";

        }
        catch (error) {
            const message = getFirebaseErrorMessage(getErrorCode(error));
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
