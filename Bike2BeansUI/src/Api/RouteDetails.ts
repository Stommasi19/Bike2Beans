import { api } from "./Client";

export const GetRoutes = async () => {
    const response = await api.get(`api/Route`)
    return response.data
}
