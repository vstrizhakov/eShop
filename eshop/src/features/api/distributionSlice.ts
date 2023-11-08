import { apiSlice } from "./apiSlice";

interface Chat {
    id: string,
    name?: string, // temp undefined
    isEnabled: boolean,
};

interface Client {
    id: string,
    firstName: string,
    lastName: string,
    isActivated: boolean,
    telegramChats: Chat[],
    viberChat: Chat,
};

export enum DeliveryStatus {
    Pending = "pending",
    Delivered = "delivered",
    Failed = "failed",
};

export interface DistributionItem {
    id: string,
    deliveryStatus: DeliveryStatus,
    viberChatId?: string,
    telegramChatId?: string,
};

interface DistributionRecipient {
    client: Client,
    items: DistributionItem[],
};

export interface Distribution {
    id: string,
    recipients: DistributionRecipient[],
};

interface GetAnnouncerInvitationResponse {
    telegram: string,
    viber: string,
};

const distributionSlice = apiSlice.injectEndpoints({
    endpoints: builder => ({
        getClients: builder.query<Client[], unknown>({
            query: () => "/distribution/clients",
            providesTags: ["clients"],
        }),
        activateClient: builder.mutation<Client, string>({
            query: clientId => ({
                url: `/distribution/clients/${clientId}/activate`,
                method: "POST",
            }),
            invalidatesTags: ["clients"],
        }),
        deactivateClient: builder.mutation<Client, string>({
            query: clientId => ({
                url: `/distribution/clients/${clientId}/deactivate`,
                method: "POST",
            }),
            invalidatesTags: ["clients"],
        }),
        
        getDistribution: builder.query<Distribution, string>({
            query: distributionId => `/distribution/distributions/${distributionId}`,
        }),
        
        getAnnouncerInvitation: builder.query<GetAnnouncerInvitationResponse, string>({
            query: announcerId => `/distribution/announcers/${announcerId}/invitation`,
        }),
    }),
});

export default distributionSlice;

export const {
    useGetClientsQuery,
    useGetDistributionQuery,
    useActivateClientMutation,
    useDeactivateClientMutation,
    useGetAnnouncerInvitationQuery,
} = distributionSlice;