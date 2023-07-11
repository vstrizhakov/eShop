import { apiSlice } from "./apiSlice";

interface Chat {
    id: string,
    name?: string, // temp undefined
    isEnabled: boolean,
};

interface Client {
    id: string,
    telegramChats: Chat[],
    viberChat: Chat,
};

enum DeliveryStatus {
    Pending = "pending",
    Delivered = "delivered",
    Failed = "failed",
};

interface DistributionItem {
    id: string,
    deliveryStatus: DeliveryStatus,
    viberChatId?: string,
    telegramChatId?: string,
};

interface Distribution {
    id: string,
    items: DistributionItem[],
}

const distributionSlice = apiSlice.injectEndpoints({
    endpoints: builder => ({
        getClients: builder.query<Client[], unknown>({
            query: () => "/distribution/clients",
        }),
        getDistribution: builder.query<Distribution, string>({
            query: distributionId => `/distribution/${distributionId}`,
        }),
    }),
});

export default distributionSlice;

export const {
    useGetClientsQuery,
    useGetDistributionQuery,
} = distributionSlice;