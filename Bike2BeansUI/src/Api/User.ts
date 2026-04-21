import { getAuth } from "firebase/auth";
import { api } from "./client";

type UserPayload = {
    email: string;
    firstName: string;
    lastName: string;
};

async function getAuthorizationHeader() {
    const user = getAuth().currentUser;
    if (!user) throw new Error("User not authenticated");

    const idToken = await user.getIdToken();
    return { Authorization: `Bearer ${idToken}` };
}

export const GetUser = async () => {
    const headers = await getAuthorizationHeader();
    const response = await api.get("api/users/get", { headers });
    return response.data;
};

export const UpdateUser = async (payload: UserPayload) => {
    const headers = await getAuthorizationHeader();
    const response = await api.put("api/users/update", payload, { headers });
    return response.data;
};

export const PatchUser = async (payload: Partial<UserPayload>) => {
    const headers = await getAuthorizationHeader();
    const response = await api.patch("api/users/update", payload, { headers });
    return response.data;
};

export const DeleteUser = async () => {
    const headers = await getAuthorizationHeader();
    const response = await api.delete("api/users/delete", { headers });
    return response.data;
};

export const CreateUser = async (firstName: string, lastName: string, email: string) => {
    const headers = await getAuthorizationHeader();
    const response = await api.post(
        "api/users/create",
        { firstName, lastName, email },
        { headers }
    );
    return response.data;
};
