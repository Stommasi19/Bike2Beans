// const BaseURL = process.env.BaseURL

import axios from "axios";

export const api = axios.create({
    baseURL: "http://localhost:5165",
    withCredentials: true,
});