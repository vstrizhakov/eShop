import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { AppThunk } from "../../app/store";

interface SignInState {
    isError?: boolean,
};

const sliceName = "signIn";

const initialState: SignInState = {
};

const slice = createSlice({
    name: sliceName,
    initialState,
    reducers: {
        setIsError: (state: SignInState, action: PayloadAction<boolean>) => {
            state.isError = action.payload;
        },
    },
});

export const savePhoneNumberForConfirmation = (phoneNumber: string): AppThunk => (dispatch, getState) => {
    localStorage.setItem("confirmation.phoneNumber", phoneNumber);
};

export const {
    setIsError,
} = slice.actions;

export default slice.reducer;