import axios, { AxiosHeaders, type InternalAxiosRequestConfig } from "axios";
import { Platform } from "react-native";
import { auth } from "../firebase";

const apiHost = Platform.OS === "android" ? "10.0.2.2" : "localhost";
const configuredApiBaseUrl =
    Platform.OS === "web" ? process.env.API_BASE_URL?.trim() : undefined;
const apiBaseUrl = configuredApiBaseUrl || `http://${apiHost}:5165`;

export const api = axios.create({
    baseURL: apiBaseUrl,
    withCredentials: true,
});

async function waitForAuthStateIfNeeded() {
    if (auth.currentUser) {
        return;
    }

    if ("authStateReady" in auth && typeof auth.authStateReady === "function") {
        await auth.authStateReady();
    }
}

async function getAuthorizationHeader() {
    await waitForAuthStateIfNeeded();

    const user = auth.currentUser;
    if (!user) {
        return {};
    }

    const idToken = await user.getIdToken();
    return { Authorization: `Bearer ${idToken}` };
}

export function withAuthorizationHeader(
    config: InternalAxiosRequestConfig,
    authorization?: string
): InternalAxiosRequestConfig {
    if (!authorization) {
        return config;
    }

    if (typeof config.headers?.set === "function") {
        config.headers.set("Authorization", authorization);
        return config;
    }

    const headers = AxiosHeaders.from(config.headers);
    headers.set("Authorization", authorization);
    config.headers = headers;

    return config;
}

api.interceptors.request.use(async (config) => {
    const { Authorization } = await getAuthorizationHeader();
    return withAuthorizationHeader(config, Authorization);
});
