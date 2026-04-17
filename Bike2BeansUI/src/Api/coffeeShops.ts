import { api } from "./Client";

export const GetCoffeeShops = async () => {
    const response = await api.get(`api/coffeeshops`)
    return response.data
}
