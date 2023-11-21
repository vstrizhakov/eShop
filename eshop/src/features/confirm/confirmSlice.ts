import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { ConfirmationLinks } from "../api/authSlice";
import { AppThunk } from "../../app/store";

interface ConfirmState {
    phoneNumber?: string,
    links?: ConfirmationLinks,
};

const sliceName = "confirm";

const initialState: ConfirmState = {
};

const slice = createSlice({
    name: sliceName,
    initialState,
    reducers: {
        setPhoneNumber: (state: ConfirmState, action: PayloadAction<string>) => {
            state.phoneNumber = action.payload;
        },
        setLinks: (state: ConfirmState, action: PayloadAction<ConfirmationLinks>) => {
            state.links = action.payload;
        },
        reset: () => initialState,
    },
});

export const readPhoneNumber = (): AppThunk => (dispath, getState) => {
    const phoneNumber = localStorage.getItem("confirmation.phoneNumber");
    if (phoneNumber) {
        dispath(slice.actions.setPhoneNumber(phoneNumber));
    }
};

export const {
    setLinks,
    reset,
} = slice.actions;

export default slice.reducer;