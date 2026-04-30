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
    const canSubmit =
        first.trim().length > 0 &&
        last.trim().length > 0 &&
        email.trim().length > 0 &&
        password.length >= 6 &&
        !loading;

    const handleSignUp = async (event: FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        if (!canSubmit) return;

        setLoading(true);
        try {
            await createUserWithEmailAndPassword(auth, email.trim(), password);
            await CreateUser(first.trim(), last.trim());
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
        if (loading) return;

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
                        <label className="loginText" htmlFor="firstname">First Name</label>
                        <input className="input"
                            type="text"
                            name="first name"
                            id="firstname"
                            placeholder="First Name"
                            autoComplete="given-name"
                            maxLength={80}
                            required
                            disabled={loading}
                            value={first}
                            onChange={(e) => setFirst(e.target.value)}
                        />
                    </div><div className="signin">
                        <label className="loginText" htmlFor="lastname">Last Name</label>
                        <input className="input"
                            type="text"
                            name="lastname"
                            id="lastname"
                            placeholder="Last Name"
                            autoComplete="family-name"
                            maxLength={80}
                            required
                            disabled={loading}
                            value={last}
                            onChange={(e) => setLast(e.target.value)}
                        />
                    </div>

                    <div className="signin">
                        <label className="loginText" htmlFor="email">Email</label>
                        <input className="input"
                            type="email"
                            name="email"
                            id="email"
                            placeholder="Email"
                            autoComplete="email"
                            inputMode="email"
                            required
                            disabled={loading}
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                        />
                    </div>
                    <div className="signin">
                        <label className="loginText" htmlFor="password">Password</label>
                        <input className="input"
                            type="password" name="password"
                            id="password"
                            placeholder="password"
                            autoComplete="new-password"
                            minLength={6}
                            required
                            disabled={loading}
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                        />
                    </div>
                    <div className="loginbtn">
                        <button className="btn-primary" type="submit" disabled={!canSubmit}>
                            {loading ? "Signing Up..." : "Sign Up"}
                        </button>

                        <button className="btn" type="button" onClick={handleGoogleSignUp} disabled={loading}>
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
