import { apiSlice } from "./apiSlice";

interface getInvitationLinkResponse {
    inviteLink: string,
};

export const telegramSlice = apiSlice.injectEndpoints({
    endpoints: builder => ({
        getInvitationLink: builder.query<getInvitationLinkResponse, unknown>({
            query: () => '/telegram/invitation',
        }),
    }),
});

export const {
    useGetInvitationLinkQuery,
} = telegramSlice;