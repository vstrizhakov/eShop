import { apiSlice } from "./apiSlice";

export const telegramSlice = apiSlice.injectEndpoints({
    endpoints: builder => ({
    }),
});

export const {
} = telegramSlice;