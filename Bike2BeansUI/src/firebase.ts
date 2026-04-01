import { initializeApp } from "firebase/app";
import { getAuth } from "firebase/auth";

const firebaseConfig = {
    apiKey: "AIzaSyDjTM2sIwEq66Ohfyz3KRd48KCI2mBvL54",
    authDomain: "bike2beans-1d091.firebaseapp.com",
    projectId: "bike2beans-1d091",
    storageBucket: "bike2beans-1d091.firebasestorage.app",
    messagingSenderId: "295985494901",
    appId: "1:295985494901:web:e7cc60122f19221772f13c",
    measurementId: "G-829Y970XRJ"
};

const app = initializeApp(firebaseConfig);

export const auth = getAuth(app);
