import { api } from "./client";

type UserPayload = {
    firstName: string;
    lastName: string;
};

export const GetUser = async () => {
    const response = await api.get("api/users/get");
    return response.data;
};

export const UpdateUser = async (payload: UserPayload) => {
    const response = await api.put("api/users/update", payload);
    return response.data;
};

export const PatchUser = async (payload: Partial<UserPayload>) => {
    const response = await api.patch("api/users/update", payload);
    return response.data;
};

export const DeleteUser = async () => {
    const response = await api.delete("api/users/delete");
    return response.data;
};

export const CreateUser = async (firstName: string, lastName: string) => {
    const response = await api.post("api/users/create", { firstName, lastName });
    return response.data;
};
