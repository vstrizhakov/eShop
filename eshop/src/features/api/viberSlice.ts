import { apiSlice } from "./apiSlice";

export const viberSlice = apiSlice.injectEndpoints({
    endpoints: builder => ({
        getViberInviteLink: builder.query({
            query: () => '/viber/invitation',
        }),
    }),
});

export const {
    useGetViberInviteLinkQuery,
} = viberSlice;