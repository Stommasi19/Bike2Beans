import { AxiosHeaders, type InternalAxiosRequestConfig } from "axios";
import { withAuthorizationHeader } from "../src/Api/client";

describe("withAuthorizationHeader", () => {
    test("normalizes plain request headers before adding authorization", () => {
        const config = {
            headers: {
                Accept: "application/json",
            },
        } as unknown as InternalAxiosRequestConfig;

        const result = withAuthorizationHeader(config, "Bearer test-token");

        expect(result.headers).toBeInstanceOf(AxiosHeaders);
        expect(result.headers.get("Accept")).toBe("application/json");
        expect(result.headers.get("Authorization")).toBe("Bearer test-token");
    });
});
