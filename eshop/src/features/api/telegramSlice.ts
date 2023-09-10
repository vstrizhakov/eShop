import { apiSlice } from "./apiSlice";

interface GetInvitationLinkResponse {
    inviteLink: string,
};

export const telegramSlice = apiSlice.injectEndpoints({
    endpoints: builder => ({
        getInvitationLink: builder.query<GetInvitationLinkResponse, string>({
            query: providerId => `/telegram/invitation/${providerId}`,
        }),
    }),
});

export const {
    useGetInvitationLinkQuery,
} = telegramSlice;