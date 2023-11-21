import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import { RootState } from "../../app/store";

const baseQuery = fetchBaseQuery({
    baseUrl: "/api",
    prepareHeaders: (headers, { getState }) => {
        const state = getState() as RootState;
        const token = state.auth.token;

        if (token) {
            headers.set("Authorization", `Bearer ${token}`);
        }

        return headers;
    },
});

export const enum ErrorCode {
    UserAlreadyExists = "userAlreadyExists",
    InvalidPassword = "invalidPassword",
    UserNotFound = "userNotFound",
};

export const apiSlice = createApi({
    baseQuery,
    tagTypes: [
        "announces",
        "categories",
        "currencies",
        "shops",
        "clients",
    ],
    endpoints: () => ({}),
});