import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { Announce } from "../../api/catalogSlice";
import { Distribution, DistributionItem } from "../../api/distributionSlice";

interface ViewAnnounceState {
    announce?: Announce,
    distribution?: Distribution,
};

const sliceName = "viewAnnounce";

const initialState: ViewAnnounceState = {
};

const slice = createSlice({
    name: sliceName,
    initialState,
    reducers: {
        setAnnounce: (state: ViewAnnounceState, action: PayloadAction<Announce>) => {
            state.announce = action.payload;
        },
        setDistribution: (state: ViewAnnounceState, action: PayloadAction<Distribution>) => {
            state.distribution = action.payload;
        },
        updateDistributionItem: (state: ViewAnnounceState, action: PayloadAction<DistributionItem>) => {
            const updatedItem = action.payload;
            const distribution = state.distribution;
            if (distribution) {
                const recipients = distribution.recipients;
                for (let i = 0; i < recipients.length; i++) {
                    const recipient = recipients[i];
                    const items = recipient.items;

                    let done = false;
                    for (let j = 0; j < items.length; j++) {
                        const item = items[j];
                        if (item.id === updatedItem.id) {
                            items[j] = updatedItem;

                            done = true;
                            break;
                        }
                    }

                    if (done) {
                        break;
                    }
                }
            }
        },
        reset: () => initialState,
    },
});

export const {
    setAnnounce,
    setDistribution,
    updateDistributionItem,
    reset,
} = slice.actions;

export default slice.reducer;