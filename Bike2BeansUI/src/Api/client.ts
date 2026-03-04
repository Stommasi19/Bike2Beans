// const BaseURL = process.env.BaseURL

import axios from "axios";
import { Platform } from "react-native";

const apiHost = Platform.OS === "android" ? "10.0.2.2" : "localhost";
const apiBaseUrl = `http://${apiHost}:5165`;

export const api = axios.create({
    baseURL: apiBaseUrl,
    withCredentials: true,
});
