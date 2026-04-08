import React, { useEffect } from "react";
import "./Toast.css";
interface ToastProps {
    message: string;
    onClose: () => void;
    duration?: number;
    type?: "error" | "success";

}

export const Toast: React.FC<ToastProps> = ({
    message,
    onClose,
    duration = 3000,
    type = "error"
}) => {
    useEffect(() => {
        const timer = setTimeout(onClose, duration);
        return () => clearTimeout(timer);
    }, [onClose, duration]);

    return (
        <div className="toast-overlay">
            <div className={`toast-popup ${type}`}>
                <p>{message}</p>
            </div>
        </div>
    );
};

export const getFirebaseErrorMessage = (code: string) => {
    console.log("Firebase error code:", code);
    switch (code) {
        case "auth/weak-password":
            return "Password must be at least 6 characters.";
        case "auth/email-already-in-use":
            return "This email is already in use.";
        case "auth/invalid-email":
            return "Please enter a valid email.";
        case "auth/popup-closed-by-user":
            return "Popup closed before completing sign up.";
        case "auth/unauthorized-domain":
            return "This domain is not authorized.";
        case "auth/invalid-credential":
            return "Invalid credential.";
        default:
            return "Something went wrong. Please try again.";
    }
};