import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { AppThunk } from "../../app/store";

interface SignUpState {
    error?: string,
};

const sliceName = "signUp";

const initialState: SignUpState = {
};

const slice = createSlice({
    name: sliceName,
    initialState,
    reducers: {
        setError: (state: SignUpState, action: PayloadAction<string>) => {
            state.error = action.payload;
        },
        reset: () => initialState,
    },
});

export const savePhoneNumberForConfirmation = (phoneNumber: string): AppThunk => (dispatch, getState) => {
    localStorage.setItem("confirmation.phoneNumber", phoneNumber);
};

export const {
    setError,
    reset,
} = slice.actions;

export default slice.reducer;