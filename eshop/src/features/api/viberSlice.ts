import { apiSlice } from "./apiSlice";

interface GetInvitationLinkResponse {
    inviteLink: string,
};

export const viberSlice = apiSlice.injectEndpoints({
    endpoints: builder => ({
        getViberInviteLink: builder.query<GetInvitationLinkResponse, string>({
            query: providerId => `/viber/invitation/${providerId}`,
        }),
    }),
});

export const {
    useGetViberInviteLinkQuery,
} = viberSlice;